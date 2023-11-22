using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
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
        this.FloatingTextPrefab.transform.position = new Vector3(this.transform.position.x, 3, this.transform.position.z); 
        
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
