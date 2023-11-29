using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Spawning / New Wave Stats")]
public class WaveStats : ScriptableObject
{
    public List<Transform> spawnPoints = new List<Transform>();
    public GameObject enemyToSpawn;
    //public float spawnFrequency;
    public int totalEnemies;
    //public int spawnedEnemies;
    public int remainingEnemies;
    





}
