using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] TMP_InputField waveInput;
    [SerializeField] TMP_Text waveTextUI;
    public List<WaveStats> waves = new List<WaveStats>();
    [SerializeField] int waveIndex;
    [SerializeField] bool waveActive;
    [SerializeField] int waveDowntime;

    //public List<Transform> spawnPoints = new List<Transform>();
    public List<GameObject> spawnedEnemies = new List<GameObject>();




    void Start()
    {
        
    }

   
    void Update()
    {
        if (waveActive)
        {
            if (waves[waveIndex].remainingEnemies <= 0)
            {
                waveActive = false;
                waveIndex++;
                StartCoroutine(WaveCooldown());
            }
        }
    }


    public void ResetWave()
    {
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            Destroy(spawnedEnemies[i]);
            spawnedEnemies[i] = null;
        }

        waveActive = false;
        StartCoroutine(WaveCooldown());
    }

    public void StartSpecificWave()
    {
        if (int.TryParse(waveInput.text, out int result))
        {
            Debug.Log("Set wave is: " + result);
            Debug.Log("Wave input modified to work with list: " + (result - 1));
            waveIndex = (result - 1);
            ResetWave();
        }
    }

    public void StartWave(int waveNum)
    {
        waveActive = true;
        waveTextUI.text = waveIndex.ToString();
        waves[waveNum].remainingEnemies = waves[waveNum].totalEnemies;
        for (int i = 0; i < waves[waveNum].totalEnemies; i++)
        {
            int spawnIndex = Random.Range(0, waves[waveNum].spawnPoints.Count);
            GameObject spawnedEnemy = Instantiate(waves[waveNum].enemyToSpawn, waves[waveNum].spawnPoints[spawnIndex].position, waves[waveNum].spawnPoints[spawnIndex].rotation);
            spawnedEnemies.Add(spawnedEnemy);
            Entity spawnedEntity = spawnedEnemy.GetComponent<Entity>();
            //spawnedEntity.attachedWave = waves[waveIndex];
        }
    }



    IEnumerator WaveCooldown()
    {
        yield return new WaitForSeconds(waveDowntime);
        //waveIndex++;
        StartWave(waveIndex);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartWave(waveIndex);
        }
    }

}
