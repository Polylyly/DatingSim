using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int dps;

    private void OnTriggerStay2D(Collider2D collision)
    {
        //collision.gameObject.GetComponent<EnemyHealth>().DamageEnemy(dps * Time.deltaTime);
    }
}
