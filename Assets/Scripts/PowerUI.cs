using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUI : MonoBehaviour
{

    int value = 10;
    

    public Text ValueText;
    void Update()
    {
        var player = GameObject.FindAnyObjectByType<unit>();
        ValueText.text = "POWER: " + player.currentPower.ToString() + "  |  GOLD: " + player.gold.ToString() + "  |  TURNS REMAINING: " + player.currentTurns.ToString();
    }
}
