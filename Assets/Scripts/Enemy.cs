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
        this.FloatingTextPrefab.transform.position = new Vector3(this.transform.position.x, 4, this.transform.position.z); 
        
    }
    public void TakeDamage(int amount)
    {
        int currentPower = this.power;
        currentPower -= amount;
        if (currentPower <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
        Debug.Log("Enemy Destroyed");
    }

    private void OnMouseOver()
    {
    
      this.FloatingTextPrefab.SetActive(true);
      this.FloatingTextPrefab.GetComponent<TextMesh>().text = "Enemy Power: " + this.power.ToString();
      this.FloatingTextPrefab.transform.LookAt(FloatingTextPrefab.transform.position - Camera.main.transform.position);
  

    }
    private void OnMouseExit()
    {
        this.FloatingTextPrefab.SetActive(false);
    }

}
