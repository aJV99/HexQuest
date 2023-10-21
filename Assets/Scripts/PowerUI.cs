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
        ValueText.text = "Power: " + player.currentPower.ToString() + " Gold: " + player.gold.ToString();
    }
}
