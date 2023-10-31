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


    public void Start()
    {
        this.FloatingTextPrefab.transform.position = new Vector3(this.transform.position.x, 3, this.transform.position.z); 
        
    }

    private void OnMouseOver()
    {
    
      this.FloatingTextPrefab.SetActive(true);
      this.FloatingTextPrefab.GetComponent<TextMeshPro>().text = "Enemy Power: " + this.power.ToString();
      this.FloatingTextPrefab.transform.LookAt(FloatingTextPrefab.transform.position - Camera.main.transform.position);
  

    }
    private void OnMouseExit()
    {
        this.FloatingTextPrefab.SetActive(false);
    }

}
