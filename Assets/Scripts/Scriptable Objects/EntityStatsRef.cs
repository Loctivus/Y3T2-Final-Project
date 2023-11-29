using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EntityStatsRef
{
    public bool useConstant = false;
    public float constantHealth;
    public float constantMana;
    public float constantBaseSpeed;
    public float constantBaseDamage;
    [Tooltip("Insert Entity Stats Variable Scriptable Object, base stats are taken from this.")]
    public EntityStatsVar esVar;

    public float maxHealthValue
    {
        get { return useConstant ? constantHealth : esVar.maxHealth; }
    }

    public float maxManaValue
    {
        get { return useConstant ? constantMana : esVar.maxMana; }
    }

    public float baseSpeedValue
    {
        get { return useConstant ? constantBaseSpeed : esVar.baseMoveSpeed; }
    }

    public float baseDamage
    {
        get { return useConstant ? constantBaseDamage : esVar.baseDamage; }
    }

}
