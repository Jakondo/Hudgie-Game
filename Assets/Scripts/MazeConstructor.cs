using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeConstructor : MonoBehaviour
{
    public bool showDebug;

    // Поля для материалов
    [SerializeField] private Material floorMat;
    [SerializeField] private Material wallMat;
    [SerializeField] private Material startMat;
    [SerializeField] private Material foodMat;

    // Объект для хранения данных лабиринта
    public int[,] data
    {
        get;
        private set;
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

    public void GenerateNewMaze(int sizeRows, int sizeCols)
    {
        //Генерация стен
        // Предупреждение о том, что лучше использовать нечетные числа для генерации лабиринта
        if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
        {
            Debug.LogError("Odd numbers work better for dungeon size.");
        }
        data = dataGenerator.FromDimensions(sizeRows, sizeCols);

        DisplayMaze();
    }

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
