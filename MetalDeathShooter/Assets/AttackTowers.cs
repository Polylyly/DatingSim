using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTowers : MonoBehaviour
{
    public float damage, hitDelay;
    bool Colliding;

<<<<<<< Updated upstream
    private void OnCollisionEnter2D(Collision2D collision)
=======
    private void OnTriggerEnter2D(Collision2D collision)
>>>>>>> Stashed changes
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            StartCoroutine(Attack(collision.gameObject));
            Colliding = true;
        }
    }

<<<<<<< Updated upstream
    private void OnCollisionExit2D(Collision2D collision)
=======
    private void OnTriggerExit2D(Collision2D collision)
>>>>>>> Stashed changes
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
