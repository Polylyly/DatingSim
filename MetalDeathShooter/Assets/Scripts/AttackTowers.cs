using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTowers : MonoBehaviour
{
    public float damage, hitDelay;
    bool Colliding;
    public PathFollow follow;
    public bool suicideBomber;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            StartCoroutine(Attack(collision.gameObject));
            Colliding = true;
            follow.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            StopCoroutine(Attack(collision.gameObject));
            Colliding = false;
            follow.enabled = true;
        }
    }

    IEnumerator Attack(GameObject Tower)
    {
        yield return new WaitForSeconds(hitDelay);
        Tower.GetComponent<TowerHealth>().DamageTower(damage);
        if(Colliding) StartCoroutine(Attack(Tower));
        if(suicideBomber)
        {
            GetComponentInChildren<BombBoat>().Explode();
        }
    }
}
