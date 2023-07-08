using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : MonoBehaviour
{
    public LayerMask detectMask;
    public float scanRadius;
    public float distance, fireDelay, arrowLifetime;
    GameObject closestEnemy;
    public GameObject Arrow;
    public Transform arrowSpawnPoint;
    bool ableToLook, ableToFire;

    // Start is called before the first frame update
    void Start()
    {
        ableToFire = true;
        ableToLook = false;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, scanRadius, detectMask);
        foreach (Collider2D col in cols)
        {
            float newDis = Vector2.Distance(transform.position, col.transform.position);
            if (distance == -1 || newDis < distance)
            {
                distance = newDis;
                closestEnemy = col.gameObject;
            }
        }
        if (distance != -1 && !ableToLook)
        {
            ableToLook = true;
            ableToFire = true;
        }
        if(distance != -1 && ableToLook)
        {
            transform.LookAt(closestEnemy.transform);
        }
        if (distance != -1 && ableToFire)
        {
            ableToFire = false;
            StartCoroutine(Fire());
        }
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(fireDelay);
        ableToLook = false;
        GameObject currentArrow = Instantiate(Arrow);
        currentArrow.transform.position = arrowSpawnPoint.position;
        currentArrow.transform.LookAt(closestEnemy.transform);
        StartCoroutine(DestroyArrow(currentArrow));
    }

    IEnumerator DestroyArrow(GameObject currentArrow)
    {
        yield return new WaitForSeconds(arrowLifetime);
        Destroy(currentArrow);
    }
}
