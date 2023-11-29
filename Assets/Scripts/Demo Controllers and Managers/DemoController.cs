using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DemoController : MonoBehaviour
{
    Material activeMat;
    public GameObject demoSpell;
    GameObject spellProjectile;
    public GameObject demoEnemy;
    [SerializeField] GameObject enemy;
    EntityMovement enemyMovement;
    Entity enemyEntity;
    public Transform spellSpawnPos;
    public Transform enemySpawnPos;
    public Transform moveTarget;
    TMP_Text enemyHealth;
    bool activeDemo;
    Spell spellRef;

    private void Awake()
    {
        activeMat = GetComponent<Renderer>().material;
        activeMat.color = Color.red;
        enemyHealth = GetComponentInChildren<TMP_Text>();
        activeDemo = true;
        ResetDemo();
        //StartCoroutine(ResetDemoTimer());
    }

    private void Update()
    {
        enemyHealth.text = "Enemy Health: " + enemyEntity.currentHealth.ToString();
    }

    public void LaunchDemoProjectile()
    {
        if (!activeDemo)
        {
            activeDemo = true;
            activeMat.color = Color.red;
            spellProjectile = Instantiate(demoSpell, spellSpawnPos.position, spellSpawnPos.rotation);
            spellRef = spellProjectile.GetComponent<Spell>();
            spellRef.demo = true;
            StartCoroutine(Launching());
        }
    }

    IEnumerator Launching()
    {
        yield return new WaitForSeconds(1.5f);
        //Spell spell = spellProjectile.GetComponent<Spell>();
        spellRef.SpellReleased();
        StartCoroutine(ResetDemoTimer());
    }

    public void MoveEnemy()
    {
        StartCoroutine(MoveDemoEnemy());
    }

    public IEnumerator MoveDemoEnemy()
    {
        yield return new WaitForSeconds(0.75f);
        activeDemo = true;
        activeMat.color = Color.red;
        enemyMovement.demoTargetPos = moveTarget.position;
        enemyMovement.isChasing = true;
        StartCoroutine(ResetDemoTimer());
    }
    
    

    public IEnumerator ResetDemoTimer()
    {
        yield return new WaitForSeconds(5f);
        ResetDemo();
    }

    public void ResetDemo()
    {
        //demoEnemy.transform.position = enemySpawnPos.position;
        Destroy(enemy);
        enemy = Instantiate(demoEnemy, enemySpawnPos.position, enemySpawnPos.rotation);
        enemyEntity = enemy.GetComponent<Entity>();
        
        //enemyHealth.text = enemyEntity.currentHealth.ToString();
        enemyMovement = enemy.GetComponent<EntityMovement>();
        enemyMovement.demoEnemy = true;
        enemyMovement.isChasing = false;
        activeDemo = false;
        activeMat.color = Color.green;
    }

}
