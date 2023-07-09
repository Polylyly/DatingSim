using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTowerBehavior : MonoBehaviour
{
    public int damage;
    public float armTime;
    public float scaleInTime;
    public ParticleSystem explosion;
    private bool exploded = false;
    void Start()
    {
        StartCoroutine(FadeIn(scaleInTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!exploded)
            {
                exploded = true;
                StartCoroutine("Explode");
            }
        }
    }

    IEnumerator Explode()
    {
        float duration = armTime;
        float timer = 0f;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            float x = 0.7f * (0.9f + (0.4f * Mathf.Sin(4 * timer)));
            transform.localScale = new Vector3(x, x, x);
            yield return null;
        }
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 1.7f);
        foreach(Collider2D col in cols)
        {
            if (col.GetComponent<HealthManager>() != null)
            {
                col.GetComponent<HealthManager>().DealDamage(damage);
            }
        }
        Destroy(this.gameObject);
        GameObject.Find("AIPlacer").GetComponent<TowerGen>().Place(transform.position);
        Instantiate(explosion, transform.position, explosion.transform.localRotation);
        yield break;
    }
    
    IEnumerator FadeIn(float duration)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Vector3 finalScale = transform.localScale;
        Vector3 initScale = new Vector3(0.01f, 0.01f, 0.01f);
        Color finalColor = Color.white;
        Color initColor = new Color(50, 70, 100);


        float timer = 0;
        while(timer < duration)
        {
            
            timer += Time.deltaTime;
            float time = Mathf.InverseLerp(0f, duration, timer);
            float x = Mathf.Lerp(initScale.x, finalScale.x, time);
            transform.localScale = new Vector3(x, x, x);
            float r = Mathf.Lerp(initColor.r, finalColor.r, time);
            float g = Mathf.Lerp(initColor.g, finalColor.g, time);
            float b = Mathf.Lerp(initColor.b, finalColor.b, time);
            Color c = new Color(r, g, b);
            renderer.color = c;
            yield return null;
        }

        yield break;
    }
}
