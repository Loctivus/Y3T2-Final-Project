using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Player / New Player Stats")]
public class PlayerStatsVar : ScriptableObject
{
    public int playerHP;
    public int playerMP;
    public float startMoveSpeed;
}
