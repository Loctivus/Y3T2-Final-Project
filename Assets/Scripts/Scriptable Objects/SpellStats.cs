using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Spells / New Spell Stats")]
public class SpellStats : ScriptableObject
{
    public enum SpellType
    {
        Fire,
        Frost,
        Arcane,
        Unholy,
    }

    public SpellType myType;

    //public float movePenalty;
    public int impactDamage;
    public int tickDamage;
    public float tickRate;
    //public Vector3 spellForce;
    public float spellDistance;
    public int manaCost;
    //public Vector3 spawnPos;

    public int spellAnimInt;
    public Sprite spellImage;
    public AudioClip castSFX;

}
