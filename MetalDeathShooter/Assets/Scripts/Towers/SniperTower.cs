using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTower : MonoBehaviour
{
    public LayerMask detectMask;
    public float scanRadius;
    private float damage = -1;
    public float fireDelay, arrowLifetime;
    GameObject closestEnemy;
    public GameObject Arrow;
    public Transform arrowSpawnPoint;
    bool ableToLook, ableToFire;
    public TroopSpawnManager troopSpawner;

    // Start is called before the first frame update
    void Start()
    {
        ableToFire = true;
        ableToLook = false;
    }

    // Update is called once per frame
    void Update()
    {
        HealthManager[] ships = troopSpawner.gameObject.transform.GetComponentsInChildren<HealthManager>();
        foreach (HealthManager att in ships)
        {
            float newDis = att.health;
            if (damage == -1 || newDis > damage)
            {
                damage = newDis;
                closestEnemy = att.gameObject;
            }
        }
        if (damage != -1 && !ableToLook)
        {
            ableToLook = true;
            ableToFire = true;
        }
        if (damage != -1 && ableToLook)
        {
            Vector2 res = closestEnemy.transform.position - transform.position;
            float angle = Vector2.Angle(Vector2.up, res);
            if (closestEnemy.transform.position.x > transform.position.x) angle *= -1;
            transform.eulerAngles = (new Vector3(0f, 0f, (angle)));
        }
        if (damage != -1 && ableToFire)
        {
            ableToFire = false;
            StartCoroutine(Fire());
        }
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(fireDelay);
        ableToLook = false;
        damage = -1;
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
