using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Подтягиваем к GameObject сразу еще один скрипт MazeGenerator
[RequireComponent(typeof(MazeConstructor))]
public class GameController : MonoBehaviour
{
    private MazeConstructor generator;
    // Start is called before the first frame update
    void Start()
    {
        // Хранение данных о компоненте MazeConstructor в generator
        generator = gameObject.GetComponent<MazeConstructor>();
        generator.GenerateNewMaze(13, 15);
    }
        
}
