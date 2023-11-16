using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PowerUI : MonoBehaviour
{    

    public Text ValueText;
    public int textSize = 16;

    // Display the players values
    void Update()
    {
        var player = GameObject.FindAnyObjectByType<unit>();
        ValueText.fontSize = textSize;

        ValueText.text = "POWER: " + player.currentPower.ToString() + "  |  GOLD: " + player.gold.ToString() + "  |  TURNS REMAINING: " + player.currentTurns.ToString() + "  |  KEYS: " + player.keys.ToString();
    }
}
