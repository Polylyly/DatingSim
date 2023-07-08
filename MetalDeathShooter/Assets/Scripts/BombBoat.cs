using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBoat : MonoBehaviour
{
    public ParticleSystem explosion;

    public void Explode()
    {
        Instantiate(explosion, transform.position, explosion.transform.localRotation);
        Destroy(gameObject);
    }
}
