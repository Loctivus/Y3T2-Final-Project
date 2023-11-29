using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingOrb : Spell
{
    #region Variables
    float age;
    Rigidbody rb;
    [Tooltip("Change to effect the rate at which the spell changes scale as it travels")]
    public AnimationCurve curve;
    SphereCollider col;
    [Tooltip("Range of spell for how far it can hit entities")]
    #endregion 

    public float explosionRadius;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
    }

    /// <summary>
    /// If no longer in hand, set its scale based on the curve and how long the projectile has been travelling
    /// </summary>
    private void Update()
    {
        if (!inHand)
        {
            float scale = curve.Evaluate(age);
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    /// <summary>
    /// If no longer in hand, calculate direction of projectile and add force to projectile
    /// If it reaches its maximum distance without hitting something, kill spell
    /// </summary>
    private void FixedUpdate()
    {
        if (!inHand)
        {
            Vector3 dir = (targetPos - transform.position).normalized;
            rb.AddForce(dir, ForceMode.Impulse);
            float travelledDist = Vector3.Distance(spawnPos, transform.position);
            age = travelledDist / spellStats.spellDistance;

            if (travelledDist > spellStats.spellDistance)
            {
                base.KillSpell(transform.position);
            }
        }
        
    }

    /// <summary>
    /// When released from hand, get its target position
    /// If the spell is a demo, use its forward transform as direction for the ray
    /// Else use center of screen for ray origin and direction
    /// Release spell from hand by setting 'inHand' to false
    /// Enable spell collider, turned off in prefab to prevent collisions with player character / casting hand
    /// Set its spawn position value for tracking spell travel distance
    /// Aim projectile at its target position
    /// </summary>
    public override void SpellReleased()
    {
        base.SpellReleased();
        Ray ray;

        if (demo)
        {
            ray = new Ray(transform.position, transform.forward);
        }
        else
        {
            //When the projectile is released, calculate the target position for the projectile to travel to when it's no longer in hand
            ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        }


        spawnPos = transform.position;
        targetPos = base.GetTargetPosition(ray);
        inHand = false;
        
        transform.LookAt(targetPos);
        col.enabled = true;
    }

    /// <summary>
    /// On release, use overlap sphere to check for entities hit on target layer
    /// Pass through info for method from spellStats scriptable object
    /// If spell can stun, raise event on event channel, passing the hit object through so it isn't raised on all entities
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {

        Collider[] entityColliders = Physics.OverlapSphere(transform.position, (explosionRadius * transform.localScale.x) / 0.75f, targetLayer);
        foreach (Collider col in entityColliders)
        {
            Debug.Log("Entity hit");
            Entity hitEntity = col.gameObject.GetComponent<Entity>();
            int scaledDamage = Mathf.RoundToInt((spellStats.impactDamage * transform.localScale.x) * 0.75f);
            Debug.Log("Damage from scaled orb: " + scaledDamage);
            hitEntity.ApplyAffliction(spellStats.myType.ToString(), scaledDamage, spellStats.tickDamage);

            if (canStun && entityStunEC != null)
            {
                entityStunEC.RaiseEvent(col.gameObject);
            }
        }

        
        base.KillSpell(transform.position);
        
    }

}
