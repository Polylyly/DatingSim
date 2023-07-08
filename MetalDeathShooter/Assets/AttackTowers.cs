using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTowers : MonoBehaviour
{
    public float damage, hitDelay;
    bool Colliding;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            StartCoroutine(Attack(collision.gameObject));
            Colliding = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            StopCoroutine(Attack(collision.gameObject));
            Colliding = false;
        }
    }

    IEnumerator Attack(GameObject Tower)
    {
        yield return new WaitForSeconds(hitDelay);
        Tower.GetComponent<TowerHealth>().DamageTower(damage);
        if(Colliding) StartCoroutine(Attack(Tower));
    }
}
