using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Entity : MonoBehaviour
{
    #region Variables
    public BoolRef gamePaused;

    [Header("Event Channels")]
    public GOEventChannelSO hitEvent;
    public GOEventChannelSO deathEvent;
    public GOEventChannelSO slowEvent;
    public GOEventChannelSO resetEvent;

    [Header ("Entity Base and Current Stats")]
    [Tooltip("Entity stats values scriptable object, new stats can be made via 'Create -> Entity -> New Entity Stats'")] 
    public EntityStatsRef entityStats;
    public float currentHealth;
    public float currentMana;
    public float currentDamage;
    public float damageMultiplier;
    public bool isDead;

    public enum SpellAffliction
    {
        None,
        Burning,
        Freezing,
        Overloading,
        Cursed,
    }

    [Header("Spell Afflictions")]
    public SpellAffliction myAffliction;
    //Affliction visual effects to be added to dictionary values
    [Tooltip("When adding/swapping affliction visual effects make sure they match the order of the affliction enum")]
    public List<GameObject> afflictionVFXs = new List<GameObject>();
    //Spawn transform for affliction visual effects
    [Tooltip("Advisable to put vfxSpawn on hips of entity to keep it around centre of body")]
    public Transform vfxSpawn;
    GameObject currentafflictVfx;
    GameObject neededAfflictVfx;
    //Dictionary to hold the affliction visual effects, enum value is the key and visual effect gameObject the value assigned with it
    Dictionary<SpellAffliction, GameObject> spawnAfflictionVFX = new Dictionary<SpellAffliction, GameObject>();
    //Duration of affliction before resetting
    [Tooltip("Time in seconds for how long the entity has the affliction when applied")]
    public float afflictionTime;


    AudioSource audioSource;
    Rigidbody rb;
    Animator anim;
    Coroutine tickRoutine;
    Coroutine afflictionTimer;
    #endregion

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        rb.velocity = new Vector3(0, 0, 0);
        currentHealth = entityStats.esVar.maxHealth;
        currentMana = entityStats.esVar.maxMana;

        //Assign values to each key in the affliction visual effect dictionary
        foreach  (GameObject afflictVFX in afflictionVFXs)
        {
            SpellAffliction spellAffliction = afflictVFX.GetComponent<AfflictionType>().afflictionType;
            spawnAfflictionVFX[spellAffliction] = afflictVFX;
        }
    }

    /// <summary>
    /// If entity does not have an affliction, set affliction to the specified spell type
    /// Also Instantiate the visual effect paired with each affliction enum value via dictionary
    /// </summary>
    /// <param name="spellType"></param>
    /// Type of spell that hit entity to determine affliction to be applied and apply effects with that affliction
    /// <param name="damage"></param>
    /// How much Damage the spell
    /// <param name="tickDamage"></param>
    public void ApplyAffliction(string spellType, int damage, int tickDamage)
    {
        if (myAffliction == SpellAffliction.None)
        {
            switch ((spellType))
            {
                case "Fire":
                    myAffliction = SpellAffliction.Burning;
                    TakeDamage(damage);
                    tickRoutine = StartCoroutine(AfflictionTicker(tickDamage, 0.25f));
                    break;

                case "Frost":
                    myAffliction = SpellAffliction.Freezing;
                    TakeDamage(damage);
                    slowEvent.RaiseEvent(gameObject);
                    break;

                case "Arcane":
                    myAffliction = SpellAffliction.Overloading;
                    currentDamage /= 2;
                    TakeDamage(damage);
                    break;

                case "Unholy":
                    myAffliction = SpellAffliction.Cursed;
                    TakeDamage(damage);
                    damageMultiplier = 2f;
                    
                    break;

                default:
                    myAffliction = SpellAffliction.None;
                    TakeDamage(damage);
                    break;
            }

            afflictionTimer = StartCoroutine(AfflictionTimer());
            
            if (spawnAfflictionVFX.TryGetValue(myAffliction, out neededAfflictVfx))
            {
                currentafflictVfx = Instantiate(neededAfflictVfx, vfxSpawn.position, vfxSpawn.rotation, vfxSpawn);
                currentafflictVfx.SetActive(true);
            }
        }
        else
        {
            AfflictionBonus(spellType, damage, tickDamage);
        }
    }

    /// <summary>
    /// Responsible for handling element combinations if the entity already has an affliction
    /// If affliction type matches the second spellType, restart and amplify the effect of that affliction
    /// Unholy Affliction is exempt from this to balance damage multiplier, unholy should be followed up on, not the follow up.
    /// </summary>
    /// <param name="spellType"></param>
    /// What spell type the hit spell was to determine affliction response
    /// <param name="damage"></param>
    /// How much damage the spell deals
    /// <param name="tickDamage"></param>
    /// How much damage the spell deals every tick at a specified rate
    void AfflictionBonus(string spellType, int damage, int tickDamage)
    {
        if (myAffliction == SpellAffliction.Burning)
        {
            switch (spellType)
            {
                //If affliction is already burning and second spellType is "Fire", restart the tick damage coroutine but with increased tick damage
                case "Fire":
                    myAffliction = SpellAffliction.Burning;
                    StopTimers();
                    tickRoutine = StartCoroutine(AfflictionTicker(tickDamage * 2, 0.25f));
                    TakeDamage(damage);
                    break;

                //If second spell type is "Frost", restart tick damage coroutine but also raise slow event on entity, combining both effects
                case "Frost":
                    StopTimers();
                    tickRoutine = StartCoroutine(AfflictionTicker(tickDamage, 0.25f));
                    slowEvent.RaiseEvent(gameObject);
                    break;

                case "Arcane":
                    StopTimers();
                    tickRoutine = StartCoroutine(AfflictionTicker(tickDamage, 0.125f));
                    TakeDamage(damage * 1.5f);
                    break;

                
                default:
                    TakeDamage(damage);
                    break;
            }

            afflictionTimer = StartCoroutine(AfflictionTimer());
        }
        else if (myAffliction == SpellAffliction.Freezing)
        {
            switch (spellType)
            {
                case "Fire":
                    StopTimers();
                    tickRoutine = StartCoroutine(AfflictionTicker(tickDamage, 0.25f));
                    slowEvent.RaiseEvent(gameObject);
                    break;

                case "Frost":
                    Debug.Log("Double slow");
                    myAffliction = SpellAffliction.Freezing;
                    StopTimers();
                    slowEvent.RaiseEvent(gameObject);
                    TakeDamage(damage);
                    break;

                case "Arcane":
                    StopTimers();
                    tickRoutine = StartCoroutine(AfflictionTicker(damage / 5, 0.125f));
                    TakeDamage(damage);
                    break;

                default:
                    TakeDamage(damage);
                    break;
            }

            afflictionTimer = StartCoroutine(AfflictionTimer());
        }
        else if (myAffliction == SpellAffliction.Overloading)
        {
            switch (spellType)
            {
                case "Fire":
                    StopTimers();
                    tickRoutine = StartCoroutine(AfflictionTicker(tickDamage, 0.125f));
                    TakeDamage(damage * 1.5f);
                    break;

                case "Frost":
                    StopTimers();
                    tickRoutine = StartCoroutine(AfflictionTicker(damage / 5, 0.125f));
                    TakeDamage(damage);
                    break;

                case "Arcane":
                    myAffliction = SpellAffliction.Overloading;
                    StopTimers();
                    currentDamage /= 4;
                    break;

                default:
                    TakeDamage(damage);
                    break;
            }

            afflictionTimer = StartCoroutine(AfflictionTimer());
        }
        else
        {
            myAffliction = SpellAffliction.None;
            TakeDamage(damage);
            damageMultiplier = 1f;
            ResetStats();
        }
    }

    /// <summary>
    /// Reduced health by damage taken times the damage multiplier value
    /// If current health is above 0, play audiosource hit sound clip
    /// Else, raise death event channel for this gameobject
    /// </summary>
    /// <param name="dmgTaken"></param>
    public void TakeDamage(float dmgTaken)
    {
        currentHealth -= dmgTaken * damageMultiplier;
        if (currentHealth <= 0)
        {
            deathEvent.RaiseEvent(gameObject);
        }
        else
        {
            audioSource.Play();
        }
    }


    /// <summary>
    /// Stop any ongoing coroutines related to affliction tick damage and affliction clear timer
    /// </summary>
    void StopTimers()
    {
        if (tickRoutine != null)
        {
            StopCoroutine(tickRoutine);
        }
        if (afflictionTimer != null)
        {
            StopCoroutine(afflictionTimer);
        }
    }

    /// <summary>
    /// Deal tick damage to entity at a rate specified by the spell stats of the spell that hit the entity and is afflicted
    /// </summary>
    /// <param name="tickDmg"></param>
    /// <param name="tickRate"></param>
    /// <returns></returns>
    IEnumerator AfflictionTicker(int tickDmg, float tickRate)
    {
        while (myAffliction != SpellAffliction.None)
        {
            TakeDamage(tickDmg);
            yield return new WaitForSeconds(tickRate);
            //tickRoutine = StartCoroutine(AfflictionTicker(tickDmg, tickRate));
        }
    }


    /// <summary>
    /// Wait in seconds the specified amount of time in afflictionTime before resetting affliction and stopping any active damage tick coroutines
    /// </summary>
    /// <returns></returns>
    IEnumerator AfflictionTimer()
    {
        yield return new WaitForSeconds(afflictionTime);
        //myAffliction = SpellAffliction.None;
        if (tickRoutine != null)
        {
            StopCoroutine(tickRoutine);
        }
        resetEvent.RaiseEvent(gameObject);
        //ResetStats();
    }


    /// <summary>
    /// Reset some entity values back to base
    /// Clear affliction back to none
    /// If an affliction visual effect object is present destroy that object
    /// </summary>
    public void ResetStats()
    {
        damageMultiplier = 1;
        myAffliction = SpellAffliction.None;
        if (currentafflictVfx != null)
        {

            Destroy(currentafflictVfx);            
        }
        currentDamage = entityStats.esVar.baseDamage;
    }
}