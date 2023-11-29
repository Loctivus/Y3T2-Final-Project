using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    #region Variables
    public bool demo;
    [Tooltip("Event channel for stunning an entity if 'canStun' is set to true")]
    public GOEventChannelSO entityStunEC;
    public bool canStun;

    [Tooltip("Spell stats scriptable object goes here, new stats can be created by going 'Create -> Spells -> New Spell Stats.")]
    public SpellStats spellStats;
    [Tooltip("Target layer for spell when identifying entities on impact.")]
    public LayerMask targetLayer;

    [Tooltip("Visual effect object when spell reaches amx distance or hits something.")]
    public GameObject impactVFX;
    public bool inHand = true;
    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public Vector3 spawnPos;
    #endregion

    public virtual void SpellReleased()
    {
        Debug.Log("Release Spell");

    }

    /// <summary>
    /// Used for calculating target position for spell projectiles
    /// </summary>
    /// <param name="ray"></param>
    /// <returns></returns>
    /// Returns the position from the raycast
    public virtual Vector3 GetTargetPosition(Ray ray)
    {
        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit))
        {
            return rayHit.point;
        }
        else
        {
            return ray.GetPoint(100f);
        }

       
    }

    /// <summary>
    /// Responsible for instantiating impact visual effects and destroying spell projectile object
    /// </summary>
    /// <param name="killPoint"></param>
    /// Position for impact visual effect to spawn at
    public virtual void KillSpell(Vector3 killPoint)
    {
        Instantiate(impactVFX, killPoint, new Quaternion(0, 0, 0, 0));
        Destroy(gameObject);
    }
}
