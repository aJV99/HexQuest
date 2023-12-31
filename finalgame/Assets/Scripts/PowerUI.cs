﻿using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUI : MonoBehaviour
{
    public TextMeshProUGUI ValueText;
    public int textSize = 16;

    // Display the player's values
    void Update()
    {
        var player = GameObject.FindObjectOfType<unit>();
        ValueText.fontSize = textSize;

        ValueText.text = "LIVES: " + player.lives.ToString() + " | POWER: " + player.currentPower.ToString() + " | GOLD: " + player.gold.ToString() + " | TURNS REMAINING: " + player.currentTurns.ToString() + " | KEYS: " + player.keys.ToString();
    }
}

