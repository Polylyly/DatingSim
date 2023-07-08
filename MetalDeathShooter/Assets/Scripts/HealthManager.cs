using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public void DealDamage(int amount)
    {
        health = Mathf.Clamp(health - amount, 0, maxHealth);
        if(health == 0f)
        {
            Destroy(this.gameObject);
        }
    }

    public void HealDamage(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }
}
