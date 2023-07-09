using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerHealth : MonoBehaviour
{
    public float maxHealth, currentHealth;
    public Action onDestroy;

    // Start is called before the first frame update
    void Start()
    {
        onDestroy += () => Kill();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            this.onDestroy();
        }
    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }
    public void DamageTower(float damage)
    {
        currentHealth -= damage;
    }
}
