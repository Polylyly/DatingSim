using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int dps;
    bool Colliding;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Damage(collision.gameObject));
            Colliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StopCoroutine(Damage(collision.gameObject));
            Colliding = false;
        }
    }

    IEnumerator Damage(GameObject Enemy)
    {
        yield return new WaitForSeconds(0.5f);
        Enemy.GetComponent<HealthManager>().DealDamage(dps);
        if (Colliding) StartCoroutine(Damage(Enemy));
    }
}
