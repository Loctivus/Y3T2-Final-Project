using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerStatsRef
{
    public bool useConstant = false;
    public float constantValue;
    public PlayerStatsVar playerStats;
    
    public float healthMaxValue
    {
        get { return useConstant ? constantValue : playerStats.playerHP; }
    }

    public float manaMaxValue
    {
        get { return useConstant ? constantValue : playerStats.playerHP; }
    }
}
