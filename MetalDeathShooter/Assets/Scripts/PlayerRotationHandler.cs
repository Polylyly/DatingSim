using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationHandler : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        float angle = Mathf.Acos(rb.velocity.normalized.y) * Mathf.Rad2Deg;
        if (rb.velocity.normalized.x > 0f) angle *= -1;
        transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }
}
