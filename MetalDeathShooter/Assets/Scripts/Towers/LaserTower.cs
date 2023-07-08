using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : MonoBehaviour
{
    public LayerMask detectMask;
    public float scanRadius;
    private float distance = -1;
    public float fireDelay, laserLifetime;
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
            Vector2 res = closestEnemy.transform.position - transform.position;
            float angle = Vector2.Angle(Vector2.up, res);
            if (closestEnemy.transform.position.x > transform.position.x) angle *= -1;
            transform.eulerAngles = (new Vector3(0f, 0f, (angle)));
        }
        if (distance != -1 && ableToFire)
        {
            ableToFire = false;
            booly = false;
            StartCoroutine(Fire());
        }
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(fireDelay);
        distance = -1;
        GameObject currentLaser = Instantiate(Laser);
        currentLaser.transform.position = laserSpawnPoint.position;
        currentLaser.transform.rotation = transform.rotation;
        currentLaser.transform.parent = transform;
        StartCoroutine(DestroyArrow(currentLaser));
    }

    IEnumerator DestroyArrow(GameObject currentLaser)
    {
        yield return new WaitForSeconds(laserLifetime);
        Destroy(currentLaser);
        booly = true;
    }
}
