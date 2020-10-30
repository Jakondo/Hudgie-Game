using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private int spawnRange = 10;
    private int foodCount;

    public List<GameObject> foodPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foodCount = GameObject.FindGameObjectsWithTag("Food").Length;
        int randIndex = Random.Range(0, foodPrefabs.Count);
        if (foodCount == 0)
        {
            Instantiate(foodPrefabs[randIndex], GenerateSpawnPosition(), foodPrefabs[randIndex].transform.rotation);
        }
    }

    Vector3 GenerateSpawnPosition()
    {
        var xValue = Random.Range(-spawnRange, spawnRange);
        var zValue = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(xValue, 0, zValue);
        return randomPos;
    }
}
