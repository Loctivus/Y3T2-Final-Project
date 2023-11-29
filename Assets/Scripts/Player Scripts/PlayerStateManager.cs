using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStateManager : MonoBehaviour
{
    [Tooltip("Player stats scriptable object goes here, new player stats scriptable object can be created via 'Create -> Player -> New Player Stats")]
    public PlayerStatsRef playerStartingStats;
    public PlayerStatsVar playerCurrentStats;

    [Header("Health and Mana UI")]
    public Slider healthSlider;
    public TMP_Text healthText;

    public Slider manaSlider;
    public TMP_Text manaText;

    private void Awake()
    {
        SetStartStats();
        InitialiseUI();
    }


    void Update()
    {
        UpdateStatInfo();
    }

    /// <summary>
    /// Set UI values to be displayed based on starting stat values
    /// </summary>
    void InitialiseUI()
    {
        healthSlider.value = playerStartingStats.playerStats.playerHP;
        manaSlider.value = playerStartingStats.playerStats.playerMP;
    }

    public void SetStartStats()
    {
        playerCurrentStats.playerMP = playerStartingStats.playerStats.playerMP;
        playerCurrentStats.playerHP = playerStartingStats.playerStats.playerHP;
    }

    /// <summary>
    /// Update UI so values match current player stats
    /// </summary>
    void UpdateStatInfo()
    {
        healthSlider.value = playerCurrentStats.playerHP;
        healthText.text = "HP: " + playerCurrentStats.playerHP + " / " + playerStartingStats.playerStats.playerHP;
        manaSlider.value = playerCurrentStats.playerMP;
        manaText.text = "HP: " + playerCurrentStats.playerMP + " / " + playerStartingStats.playerStats.playerMP;
    }

}
