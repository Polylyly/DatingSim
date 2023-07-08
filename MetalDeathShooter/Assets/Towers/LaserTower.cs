using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : MonoBehaviour
{
    public LayerMask detectMask;
    public float scanRadius;
    public float distance, fireDelay, laserLifetime;
    GameObject closestEnemy;
    public GameObject Laser;
    public Transform laserSpawnPoint;
    bool ableToLook, ableToFire, booly;

    // Start is called before the first frame update
    void Start()
    {
        ableToFire = true;
        ableToLook = true;
        booly = true;
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
        if (distance != -1 && booly)
        {
            ableToLook = true;
            ableToFire = true;
        }
        if (distance != -1 && ableToLook)
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
        booly = false;
        GameObject currentLaser = Instantiate(Laser);
        currentLaser.transform.position = laserSpawnPoint.position;
        StartCoroutine(DestroyArrow(currentLaser));
    }

    IEnumerator DestroyArrow(GameObject currentLaser)
    {
        yield return new WaitForSeconds(laserLifetime);
        Destroy(currentLaser);
        booly = true;
    }
}
