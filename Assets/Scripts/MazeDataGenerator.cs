using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDataGenerator : MonoBehaviour
{
    public float placementTreshold; // Переменная для определения пустого пространства в лабиринте
    
    public MazeDataGenerator()
    {
        placementTreshold = .1f;
    }

    public int[,] FromDimensions(int sizeRows, int sizeCols)
    {
        int[,] maze = new int[sizeRows, sizeCols];

        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                // Проверка на предельные значения сетки, если крайние значения пустые то заменяется на 1, то есть ставится стенка
                if (i == 0 || j == 0 || i == rMax || j == cMax)
                {
                    maze[i, j] = 1;
                }

                // Проверка на то, делятся ли значения координат нацело и ячейки со значением placementTrershold
                else if (i % 2 == 0 && j % 2 == 0)
                {
                    if (Random.value > placementTreshold)
                    {
                        //Случайная генерация стенок внутри массива
                        maze[i, j] = 1;

                        int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                        int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                        maze[i + a, j + b] = 1;
                    }
                }
            }
        }
        return maze;
    }
}
