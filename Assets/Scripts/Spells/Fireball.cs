using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell
{
    #region Variables
    Rigidbody rb;
    SphereCollider col;
    [Tooltip("Range of spell for how far it can hit entities")]
    public float explosionRadius;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
    }

    /// <summary>
    /// If no longer in hand, add downward force to the projectile, causing it to have an arc in its trajectory
    /// </summary>
    private void FixedUpdate()
    {
        if (!inHand)
        {
            rb.AddForce(Vector3.down * Physics.gravity.magnitude * rb.mass);
        }
    }
    /// <summary>
    /// If the spell is a demo, use its forward transform as direction for the ray
    /// Else use center of screen for ray origin and direction
    /// Release spell from hand by setting 'inHand' to false
    /// Add a singular impulse force to object, allows for better curving on its trajectory due to downward force in FixedUpdate
    /// Enable spell collider, turned off in prefab to prevent collisions with player character / casting hand
    /// </summary>
    public override void SpellReleased()
    {
        base.SpellReleased();
        Ray ray;

        if (demo)
        {
            rb.AddForce(transform.forward * 25f, ForceMode.Impulse);
        }
        else
        {
            //When the projectile is released, calculate the target position for the projectile to travel to when it's no longer in hand
            ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            targetPos = base.GetTargetPosition(ray);
        }


        inHand = false;
        if (!demo)
        {
            Vector3 dir = (targetPos - transform.position).normalized;
            rb.AddForce(dir * 25f, ForceMode.Impulse);
        }
        col.enabled = true;
    }

    /// <summary>
    /// On release, use overlap sphere to check for entities hit on target layer
    /// Pass through info for method from spellStats scriptable object
    /// If spell can stun, raise event on event channel, passing the hit object through so it isn't raised on all entities
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        Collider[] entityColliders = Physics.OverlapSphere(transform.position, explosionRadius, targetLayer);
        
        foreach (Collider col in entityColliders)
        {
            Entity hitEntity = col.gameObject.GetComponent<Entity>();
            hitEntity.ApplyAffliction(spellStats.myType.ToString(), spellStats.impactDamage, spellStats.tickDamage);

            if (canStun && entityStunEC != null)
            {
                entityStunEC.RaiseEvent(col.gameObject);

            }
        }
        base.KillSpell(transform.position);
    }
}
