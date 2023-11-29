using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityMovement : MonoBehaviour
{
    #region Variables
    [Tooltip("Entity stats values scriptable object, new stats can be made via 'Create -> Entity -> New Entity Stats'")]
    public EntityStatsRef entityStats;

    [Header("AI Navigation")]
    public Vector3Ref targetPos;
    [HideInInspector] public Vector3 demoTargetPos;
    public bool demoEnemy;
    public float minDistance;
    public float checkTargetPathInterval;
    //public BoolRef entityDead;
    NavMeshAgent nmAgent;
    NavMeshPath nmPath;
    [SerializeField] BoolVariable gamePaused;
    [Header("Entity states")]
    [SerializeField] bool canAttack;
    public bool isChasing;
    [SerializeField] bool isSlowed;
    [SerializeField] bool isStunned;
    Animator anim;

    #endregion

    private void Awake()
    {
        nmAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// If in chase, move entity towards its target position
    /// IF entity is a demo enemy, its target position is set by the demo controller
    /// If not demo enemy, move towards player position, as set by the Player Position scriptable object
    /// </summary>
    void Update()
    {
        if (!gamePaused.value)
        {
            if (!isStunned && isChasing && !canAttack)
            {
                //anim.SetBool("Chasing_b", true);
                float targetDist;

                if (!demoEnemy)
                {
                    targetDist = Vector3.Distance(transform.position, targetPos.value);
                    if (targetDist > minDistance)
                    {
                        nmAgent.destination = targetPos.value;
                        anim.SetBool("Chasing_b", true);

                    }
                    else
                    {
                        canAttack = true;
                        anim.SetBool("Chasing_b", false);
                    }
                }
                else
                {
                    targetDist = Vector3.Distance(transform.position, demoTargetPos);
                    if (targetDist > minDistance)
                    {
                        nmAgent.destination = demoTargetPos;
                        anim.SetBool("Chasing_b", true);

                    }
                    else
                    {
                        canAttack = true;
                        anim.SetBool("Chasing_b", false);
                    }
                }
            }
        }
    }


    /// <summary>
    /// Stop entity if stunned by spell
    /// set new destination to prevent it from still moving while stunned
    /// </summary>
    public void StunEntity()
    {
        isStunned = true;
        nmAgent.destination = transform.position;
        anim.SetTrigger("Damaged_t");
        StartCoroutine(StunTimer());
    }

    public IEnumerator StunTimer()
    {
        yield return new WaitForSeconds(2f);
        isStunned = false;
    }


    /// <summary>
    /// When slowed, set movement speed to be 1/4 of base speed, set the anim speed multiplier to match new speed
    /// If already slowed, set movement and animation speed multiplier to be lower
    /// </summary>
    public void Slowed()
    {
        if (isSlowed)
        {
            anim.SetFloat("MoveSpeedMultiplier_f", 0.125f);
            nmAgent.speed = entityStats.baseSpeedValue / 8;
        }
        else
        {
            anim.SetFloat("MoveSpeedMultiplier_f", 0.25f);
            nmAgent.speed = entityStats.baseSpeedValue / 4;
            isSlowed = true;
        }
        
    }

    /// <summary>
    /// Reset entity movement speed and animation speed multiplier back to base values
    /// </summary>
    public void ResetMovement()
    {
        nmAgent.speed = entityStats.baseSpeedValue;
        anim.SetFloat("MoveSpeedMultiplier_f", 1f);
    }


    public void ActivateMovement()
    {
        isChasing = true;
    }

    public void DeactivateMovement()
    {
        isChasing = false;
    }



}
