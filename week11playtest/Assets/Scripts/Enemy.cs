using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

[SelectionBase]

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public int power;

    [SerializeField]
    private GameObject FloatingTextPrefab;

    [SerializeField]
    public bool isBoss;


    public void Start()
    {
        this.FloatingTextPrefab.transform.position = new Vector3(this.transform.position.x, 6, this.transform.position.z);
        Difficulty currentDifficulty = GlobalSettings.SelectedDifficulty;

        AdjustPowerBasedOnDifficulty(currentDifficulty);

    }

    private void AdjustPowerBasedOnDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                power = Mathf.CeilToInt(power * 0.5f); // Half power for easy
                break;
            case Difficulty.Medium:
                // No change for medium
                break;
            case Difficulty.Hard:
                power = power * 2; // Double power for hard
                break;
        }
    }

    private void OnMouseOver()
    {
    
      this.FloatingTextPrefab.SetActive(true);
        if (this.isBoss)
        {
            this.FloatingTextPrefab.GetComponent<TextMeshPro>().text = "Boss Power: " + this.power.ToString();

        }
        else
        {
            this.FloatingTextPrefab.GetComponent<TextMeshPro>().text = "Enemy Power: " + this.power.ToString();

        }
        // Make the text look at the camera
        Vector3 directionToCamera = FloatingTextPrefab.transform.position - Camera.main.transform.position;

        // Zero out the y component of the direction vector to make sure it doesn't tilt
        directionToCamera.y = 0;

        // Update the rotation of the text to face the camera, only around the Y-axis
        FloatingTextPrefab.transform.rotation = Quaternion.LookRotation(directionToCamera);



    }
    private void OnMouseExit()
    {
        this.FloatingTextPrefab.SetActive(false);
    }

}
