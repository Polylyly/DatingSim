using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : MonoBehaviour
{
    public LayerMask detectMask;
    public float scanRadius;
    private float distance = -1;
    public float fireDelay, arrowLifetime;
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
            /**
            Quaternion rotation = Quaternion.LookRotation
            (closestEnemy.transform.position - transform.position, transform.TransformDirection(Vector3.up));
            transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
            */
            Vector2 res = closestEnemy.transform.position - transform.position;
            float angle = Vector2.Angle(Vector2.up, res);
            if (closestEnemy.transform.position.x > transform.position.x) angle *= -1;
            transform.eulerAngles = (new Vector3(0f, 0f, (angle)));
            
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
        distance = -1;
        GameObject currentArrow = Instantiate(Arrow);
        currentArrow.transform.position = arrowSpawnPoint.position;
        currentArrow.transform.rotation = transform.rotation;
        StartCoroutine(DestroyArrow(currentArrow));
    }

    IEnumerator DestroyArrow(GameObject currentArrow)
    {
        yield return new WaitForSeconds(arrowLifetime);
        Destroy(currentArrow);
    }
}
