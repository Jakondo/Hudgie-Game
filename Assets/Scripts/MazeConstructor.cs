using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeConstructor : MonoBehaviour
{
    public bool showDebug;

    // Поля для материалов
    //[SerializeField] private Mesh meshObject;
    [SerializeField] private Material floorMat;
    [SerializeField] private Material wallMat;
    [SerializeField] private Material startMat;
    [SerializeField] private GameObject foodObject;
    [SerializeField] private Material foodMat;

    // Объект для хранения данных лабиринта
    public int[,] data
    {
        get; private set;
    }

    // Ширина и высота коридоров
    public float hallWidth
    {
        get; private set;
    }
    public float hallHeight
    {
        get; private set;
    }

    // Координаты стартовой позиции
    public int startRow
    {
        get; private set;
    }
    public int startCol
    {
        get; private set;
    }

    // Координаты собираемого предмета
    public int goalRow
    {
        get; private set;
    }
    public int goalCol
    {
        get; private set;
    }

    private MazeDataGenerator dataGenerator;
    private MazeMeshGenerator meshGenerator;


    private void Awake()
    {
        dataGenerator = new MazeDataGenerator();
        meshGenerator = new MazeMeshGenerator();

        // Примитивный лабиринт с пустым центром
        data = new int[,]
        {
            {1, 1, 1},
            {1, 0, 1},
            {1, 1, 1}
        };        
    }

    public void GenerateNewMaze(int sizeRows, int sizeCols, 
        TriggerEventHandler startCallback=null, TriggerEventHandler goalCallback=null)
    {
        //Генерация стен
        // Предупреждение о том, что лучше использовать нечетные числа для генерации лабиринта
        if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
        {
            Debug.LogError("Odd numbers work better for dungeon size.");
        }

        DisposeOldMaze();

        data = dataGenerator.FromDimensions(sizeRows, sizeCols);

        FindStartPosition();
        FindGoalPosition();

        // store values used to generate this mesh
        hallWidth = meshGenerator.width;
        hallHeight = meshGenerator.height;

        DisplayMaze();

        PlaceStartTrigger(startCallback);
        PlaceGoalTrigger(goalCallback);
    }

    // Уничтожение лабиринта
    public void DisposeOldMaze()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Generated");
        foreach (GameObject go in objects)
        {
            Destroy(go);
        }
    }

    // Поиск стартовой позиции
    private void FindStartPosition()
    {
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                if (maze[i, j] == 0)
                {
                    startRow = i;
                    startCol = j;
                    return;
                }
            }
        }
    }

    // Поиск позиции собираемого предмета
    private void FindGoalPosition()
    {
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        // loop top to bottom, right to left
        for (int i = rMax; i >= 0; i--)
        {
            for (int j = cMax; j >= 0; j--)
            {
                if (maze[i, j] == 0)
                {
                    goalRow = i;
                    goalCol = j;
                    return;
                }
            }
        }
    }

    // Создание визуального элемента начальной точки
    private void PlaceStartTrigger(TriggerEventHandler callback)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = new Vector3(startCol * hallWidth, .5f, startRow * hallWidth);
        go.name = "Start Trigger";
        go.tag = "Generated";

        go.GetComponent<BoxCollider>().isTrigger = true;
        go.GetComponent<MeshRenderer>().sharedMaterial = startMat;

        TriggerEventRouter tc = go.AddComponent<TriggerEventRouter>();
        tc.callback = callback;
    }

    // Создание визуального элемента конечной точки
    private void PlaceGoalTrigger(TriggerEventHandler callback)
    {
        GameObject go = GameObject.Instantiate(foodObject, new Vector3(goalCol * hallWidth, .5f, goalRow * hallWidth), foodObject.transform.rotation);        
        go.transform.position = new Vector3(goalCol * hallWidth, .5f, goalRow * hallWidth);

        go.name = "Treasure";
        go.tag = "Generated";        

        go.GetComponent<BoxCollider>().isTrigger = true;
        go.GetComponent<MeshRenderer>().sharedMaterial = foodMat;

        TriggerEventRouter tc = go.AddComponent<TriggerEventRouter>();
        tc.callback = callback;
    }

    // Отображение сгенерированного лабиринта
    public void DisplayMaze()
    {
        GameObject go = new GameObject();
        go.transform.position = Vector3.zero;
        go.name = "Procedural Maze";
        go.tag = "Generated";

        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = meshGenerator.FromData(data);

        MeshCollider mc = go.AddComponent<MeshCollider>();
        mc.sharedMesh = mf.mesh;

        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.materials = new Material[2] { floorMat, wallMat };
    }

    private void OnGUI()
    {
        // Проверка на включенный режим отладки
        if (!showDebug)
        {
            return;
        }

        // Переменная хранящая данные лабиринта, Максимальное значение строки, Максимальное значение столбца, Строка для вывода информации
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        string msg = "";

        // Два цикла для проверки значений в строках и столбцах и отрисовки соответствующих элементов
        for (int i = rMax; i >= 0; i--)
        {
            for (int j = 0; j <= cMax; j++)
            {
                if (maze[i,j] == 0)
                {
                    msg += "....";
                }
                else
                {
                    msg += "==";
                }
            }
            msg += "\n";
        }
        // Визуализация полученного результата
        GUI.Label(new Rect(20, 50, 500, 500), msg);
    }
}
