using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MagicMissile : Spell
{
    #region Variables
    Rigidbody rb;
    SphereCollider col;
    #endregion

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
    }

    /// <summary>
    /// When no longer in hand, add force to its forward transform, projectile is already rotated to have its forward be towards the target position
    /// update its travelled distances based on its spawn position and current position
    /// If the distance is greated than the distance the spell can travel via tis spell stats scriptable object, call KillSpell to destroy spell object
    /// </summary>
    private void FixedUpdate()
    {
        if (!inHand)
        {
            rb.AddForce(transform.forward, ForceMode.Impulse);

            float travelledDist = Vector3.Distance(spawnPos, transform.position);

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
        //Debug.Log("Check to make sure it breaks properly if its a demo spell");
        if (demo)
        {
            ray = new Ray(transform.position, transform.forward);

        }
        else
        {
            ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            
        }
        spawnPos = transform.position;
        targetPos = GetTargetPosition(ray);
        inHand = false;
        transform.LookAt(targetPos);
        col.enabled = true;
    }

    /// <summary>
    /// If collided object is on 'Enemy Entity' layer, get its entity component and call method for applying affliction
    /// Pass through info for method from spellStats scriptable object
    /// If spell can stun, raise event on event channel, passing the hit object through so it isn't raised on all entities
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy Entity"))
        {
            Entity hitEntity = collision.gameObject.GetComponent<Entity>();
            hitEntity.ApplyAffliction(spellStats.myType.ToString(), spellStats.impactDamage, spellStats.tickDamage);
            if (canStun && entityStunEC != null)
            {
                entityStunEC.RaiseEvent(collision.gameObject);
            };
        }
        
        base.KillSpell(transform.position);
    }

}
