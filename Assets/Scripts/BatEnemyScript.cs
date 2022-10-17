using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnemyScript : MonoBehaviour
{
    public float speed;
    private GameObject player;
    public Transform startPoint;

    public bool chase;

    [Header("Bite")]
    public bool isBite;
    public Transform attackPoint;
    public float attackrange = 0.3f;
    public LayerMask BitePlayer;
    public float attackDistance; //khoảng cách tối thiểu để tấn công
                                 // public GameObject HITEffect;
    public float intTimer;
    public float timercooldame =2f;

    public AudioClip biteClip;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if(player == null)
        {
            return; 
        }


        if (chase == true) Chase();
        else returnStartPoint();
        
        Flip();
        _bitePlayer();
        Cooldown();

    }

    void _bitePlayer()
    {   
         float distance = Vector2.Distance(transform.position, player.transform.position);
        if(attackDistance >= distance && isBite ==false)
        {
            isBite = true;
           
            Collider2D[] bitePlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackrange, BitePlayer);
            foreach (Collider2D player in bitePlayer)
            {
                if (player.CompareTag("Player"))
                {
                    AudioScripts.instance.playSound(biteClip);
                    //Instantiate(HITEffect, enemy.GetComponent<EnemyScript>().transform.position, transform.rotation);
                    player.GetComponent<HealthBar>().loseHeadth(5);
                   
                }
            }
        }
    }

    void Cooldown()
    {
        intTimer -= Time.deltaTime;

        if (intTimer <= 0 && isBite)
        {
            isBite = false;
            intTimer = timercooldame;
        }
    }

    //truy đuổi player
    private void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed*Time.deltaTime);
    }

    //trở về vị trí
    private void returnStartPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, startPoint.transform.position, speed * Time.deltaTime);
    }


    void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > player.transform.position.x)
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }
        transform.eulerAngles = rotation;
    }
    private void OnDrawGizmosSelected()
    {
       
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPoint.position, attackrange);
    }



}
