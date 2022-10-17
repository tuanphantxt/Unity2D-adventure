using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    
    private bool islive;
    public Transform target;
    public float moveSpeed;
    public float distance;
    public float attackDistance;
    public float shootAttack;
    public GameObject butletPrefabsBoss;
    public Transform firePoint;
    public Collider2D coliderBoss;
    public float intTimer;
    public float timercooldame;
    public int damage;

    private bool canAttack;
    private Animator anim;
    public bool inRange;
    private bool canshoot = true;
    private float shootTime;
    private float shootTimeLife = 2f;

    public Transform attackPoint;
    public float attackrange = 0.3f;
    public LayerMask BitePlayer;
    public GameObject Cua;
    public GameObject CuaChan;
    public Transform point;
    public AudioClip attackClip, fireBallClip, dieClip;



    // Start is called before the first frame update
    void Start()
    {
        islive = true;
        canAttack = true;
        anim = GetComponent<Animator>();
        target = point;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<EnemyHealBar>().healht <= 0)
        {
            coliderBoss.enabled = false;
            islive = false;
            anim.Play("death");
           // AudioScripts.instance.playSound(dieClip);
        }
        if (islive)
        {
            Cua.SetActive(true);
            coliderBoss.enabled = true;
            if(inRange)Flip();
            EnemyLogic();

            if (!canshoot)
            {
                CooldownShoot();
            }
        }else
        {
            Cua.SetActive(false);
            CuaChan.SetActive(false);
        }
        

        if (!canAttack)
        {
            Cooldown();
        }
    }
    /*void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            inRange = true;
            target = trig.transform;//nhận đối tượng dể di chuyển
            Debug.Log("Đã thấy đối tượng");
        }
    }*/


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = true;
            target = collision.transform;
            CuaChan.SetActive(true);
            Debug.Log("đã thấy đối tượng");
           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    public void _bitePlayer()
    {
        canAttack = false;
            Collider2D[] bitePlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackrange, BitePlayer);
            foreach (Collider2D player in bitePlayer)
            {
                if (player.CompareTag("Player"))
                {
                    //Instantiate(HITEffect, enemy.GetComponent<EnemyScript>().transform.position, transform.rotation);
                    player.GetComponent<HealthBar>().loseHeadth(damage);
                }
            }
        
    }
   
    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);//nhận khoảng cách giữa nó và vị trí targets(player)
        if(inRange)
        {
            if (distance > shootAttack && inRange)
            {
                StopAttack();
                Move();
            }
            if (distance > attackDistance && !canshoot && inRange)
            {
                StopAttack();
                Move();
            }
            else if (attackDistance >= distance && inRange)
            {
                Attack();
            }
            else if (attackDistance >= distance && !canAttack && inRange)
            {
                Shoot();
                canshoot = false;
            }
            else if (shootAttack >= distance && canshoot && inRange)
            {
                Shoot();
                canshoot = false;
            }
        }
        
    }


    //set hoạt ảnh shoot
    public void Shoot()
    {
        anim.Play("Shoot");  
    }

    //hàm shoot dùng trong animation
    public void shootButllet()
    {
        AudioScripts.instance.playSound(fireBallClip);
        Instantiate(butletPrefabsBoss, firePoint.position, firePoint.rotation);
    }

    //set hoạt ảnh attack
    void Attack()
    {
     if(canAttack)
        anim.SetBool("Attack", true);
     else anim.SetBool("Attack", false);
    }

    //ngưng tấn công 
    void StopAttack()
    {
        //anim.SetBool("Shoot", false);
        anim.SetBool("Attack", false);
    }


    //hồi chiêu
    void Cooldown()
    {
        intTimer -= Time.deltaTime;

        if (intTimer <= 0 && !canAttack)
        {
            canAttack = true;
            intTimer = timercooldame;
        }
    }
    //hàm di chuyển
    void Move()
    {
        anim.SetBool("Move", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Shoot")) //nếu đang không thực hiện hành động này
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            // di chuyển sprite đến vị trí đích 
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (inRange) transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * 1.5f * Time.deltaTime);
        }
    }
    //hồi range attack
    void CooldownShoot()
    {
        shootTime -= Time.deltaTime;
        
        if (shootTime <= 0)
        {
            canshoot = true;
            shootTime = shootTimeLife;
        }
    }

    //transform.eulerAngles đại diện cho sự quay trong không gian thế giới.
    private void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }
        transform.eulerAngles = rotation;
    }

    public void attacksoud()
    {
        AudioScripts.instance.playSound(attackClip);
    }
    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPoint.position, attackrange);
    }
}
