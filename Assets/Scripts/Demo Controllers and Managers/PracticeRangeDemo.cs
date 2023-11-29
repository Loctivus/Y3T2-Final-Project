using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeRangeDemo : MonoBehaviour
{
    Material activeMat;
    public GameObject enemyToSpawn;
    public int numToSpawn;
    public List<Transform> spawnPoints = new List<Transform>();
    [SerializeField]List<GameObject> spawnedEnemies = new List<GameObject>();


    public void Awake()
    {
        ResetDemo();
    }

    public void ResetDemo()
    {
        if (spawnedEnemies != null)
        {
            foreach (GameObject enemy in spawnedEnemies)
            {
                Destroy(enemy);
            }
            spawnedEnemies.Clear();
        }

        for (int i = 0; i < numToSpawn; i++)
        {
            GameObject newEnemy = Instantiate(enemyToSpawn, spawnPoints[i].position, spawnPoints[i].rotation);
            spawnedEnemies.Add(newEnemy);
        }
    }


}
