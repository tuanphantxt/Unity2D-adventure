using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyButllet : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;
    public int damage = 2;
    public GameObject impactEffect;
    // Start is called before the first frame update
    void Start()
    {

        rb.velocity = transform.right * speed;

    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        HealthBar enemy = hitInfo.GetComponent<HealthBar>();
        if (enemy != null)
        {
            enemy.loseHeadth(damage);
        }
        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
