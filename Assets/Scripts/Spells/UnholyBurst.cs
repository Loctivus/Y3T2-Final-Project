using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnholyBurst : Spell
{
    [Tooltip("Range of spell for how far it can hit entities")]
    public float explosionRadius;

    /// <summary>
    /// On release, use overlap sphere to check for entities hit on target layer
    /// Pass through info for method from spellStats scriptable object
    /// If spell can stun, raise event on event channel, passing the hit object through so it isn't raised on all entities
    /// </summary>
    public override void SpellReleased()
    {
        base.SpellReleased();
        transform.rotation = new Quaternion(0, 0, 0, 0);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, targetLayer);
        foreach(Collider col in hitColliders)
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
