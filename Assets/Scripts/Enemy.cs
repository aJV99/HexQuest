using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int power;

    public void TakeDamage(int amount)
    {
       this.power -= amount;
       if (this.power <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
        Debug.Log("Enemy Destroyed");
    }
}
