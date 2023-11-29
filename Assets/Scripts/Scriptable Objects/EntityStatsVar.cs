using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Entity / New Entity Stats")]
public class EntityStatsVar : ScriptableObject
{
    public float maxHealth;
    public float maxMana;
    public float baseMoveSpeed;
    public float baseDamage;
}
