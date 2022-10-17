using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButletScript : MonoBehaviour
{

    public float speed = 20f;
    public Rigidbody2D rb;
    public int damage = 2;
    public GameObject impactEffect;
    public AudioClip hitClip;
    // Start is called before the first frame update
    void Start()
    {
       
            rb.velocity = transform.right * speed;
        
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        EnemyHealBar enemy = hitInfo.GetComponent<EnemyHealBar>();
        if(enemy  !=null)
        {
            enemy.takeDame(damage);
        }
        Instantiate(impactEffect, transform.position, transform.rotation);
        Debug.Log(hitInfo.name);
        Destroy(gameObject);
        AudioScripts.instance.playSound(hitClip);
    }
}
