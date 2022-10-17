using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("abc")]
    public float horizontalInput;
    [SerializeField] private float _playerSpeed = 10f;//tốc độ chạy
    [SerializeField] private float _playerJump = 12f;//khoảng cách nhảy
    [SerializeField] private int _ExtraJump = 2;//số lần nhảy


    [SerializeField] private bool isGround = false;//có chạm đất k
    [SerializeField] private bool crouch = false;
    [SerializeField] public bool isSliding;
    [SerializeField] private float slideFactor = 0.2f;
    [SerializeField] private bool isDead;

    public float Dicrection => horizontalInput;

    [Header("Dash")]
    public bool isDashing;
    private bool canDash = true;
    private bool m_facing_right;
    public float dashingTime = 0.2f; //thời gian lướt
    [SerializeField] private float dashForce = 25f;//lực
    public float isDashingColldown = 1f;// ngăn chặn dash liên tục
    [SerializeField] private TrailRenderer tr;


    private Rigidbody2D _playerRb;
    public Animator anim;
    public GameObject Player;

    [Header("check Grond wall")]
    [SerializeField] Collider2D standingCollider;//full body collider
    [SerializeField] Collider2D crouchCollider;//full body collider

    [SerializeField] private Transform groundCheckCollider;
    [SerializeField] private Transform overheadCheckCollider;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] private Transform wallCheckCollider;
    [SerializeField] LayerMask wallLayer;
    const float overheadCheckRadius = 0.2f;//check head
    const float groundCheckRadius = 0.2f;//check ground
    const float wallCheckRadius = 0.2f;//check wall

    [Header("Melee attack")]
    public bool isAttacking;
    public static playerController instance;
    public Transform attackPoint;
    public float attackrange = 0.5f;
    public LayerMask Enemy;
    public GameObject HITEffect;


    [Header("range attack")]
    public Transform firePoint;
    public GameObject butletPrefabs;
    public GameObject kamejoko;
    private bool israngeAttack;


    [Header("cooldown")]
    public float KameTimeLife = 2f;
    public float KameTime;
    public bool isKame;

    public float MelleTimeLife = 2f;
    public float MelleTime;
    public bool isMelle;
    public float DashTimeLife = 2f;
    public float DashTime;
    public bool cooldowndash;


    [Header("sound")]
    [SerializeField] private AudioClip walkClip, attackClip, kameClip, dashClip, jumpClip;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        _playerRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {
       
        attack();//tấn công
        horizontalInput = Input.GetAxis("Horizontal");
        Movement();//di chuyển
        GroundCheck();//check dứng trên dất
        wallCheck();//trượt tường
        playerRespawn();//hồi sinh
         if(!isKame)
        {
            cooldownKame();
        }
        if (!isMelle)
        {
            cooldownMelleAttack();
        }
        if (!cooldowndash)
        {
            cooldownDash();
        }
    }

 

    void attack()
    {
        //melee attack
        if(Input.GetKeyDown(KeyCode.J) && isGround && !isAttacking  && israngeAttack == false)
        {
            MeleeAttack();  
        }
        //range attack
        if (Input.GetKeyDown(KeyCode.K) && isSliding == false && isKame)
        {
            israngeAttack = true;
            kamejoko.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.K) && FindObjectOfType<playerController>())
        {
            israngeAttack = false;
            kamejoko.SetActive(false);
            Shoot();
        }
    }

 
    void Movement()
    {
        if (!canMove()) return;//khóa di chuyển
        if(isDead) return; //lập lại hồi sinh
        if(isDashing) return;//lập lại dash


        //movement
        horizontalInput = Input.GetAxis("Horizontal");

        _playerRb.velocity = new Vector2(horizontalInput * _playerSpeed, _playerRb.velocity.y);
        //sau khi đi ra khỏi vật thể bị chắn trên đầu nếu k còn ấn nút Z sẽ tự động hủy crouch
        if(horizontalInput !=0 && Input.GetKey(KeyCode.C) == false && Input.GetKey(KeyCode.C) == false && Input.GetKey(KeyCode.DownArrow) == false && Input.GetKey(KeyCode.S) == false)
        {
            //nếu nhảy mà vẫn còn vật thể chặn trên đầu thì hiêu ứng crown vẫn còn
            if (Physics2D.OverlapCircle(overheadCheckCollider.position, overheadCheckRadius, groundLayer))
            {
                _crouchTrue();
            }else _crouchFalse();
        }
        

        //trong trạng thái crouch tốc độ di chuyển sẽ giảm
        if (crouch)
        {
            _playerRb.velocity = new Vector2(horizontalInput * (_playerSpeed /3), _playerRb.velocity.y);
        }

        //bật anim Run nếu horizontalInput >0  Mathf.Abs() bỏ giá trị âm
        anim.SetFloat("Run", Mathf.Abs(horizontalInput));

        // gán vị trí lên xuống cho animator jump fall
        anim.SetFloat("yVelocity", _playerRb.velocity.y);




        //jump
        if (Input.GetKeyDown(KeyCode.Space) && _ExtraJump >1 
            || Input.GetKeyDown(KeyCode.W) && _ExtraJump > 1 
            || Input.GetKeyDown(KeyCode.UpArrow) && _ExtraJump > 1)
        {
            
            //nếu nhảy mà vẫn còn vật thể chặn trên đầu thì hiêu ứng crown vẫn còn
            if (Physics2D.OverlapCircle(overheadCheckCollider.position, overheadCheckRadius, groundLayer))
            {
                _crouchTrue();
            }
            else 
            {
                //nhảy sẽ tắt crouch
                _crouchFalse();
                _playerRb.velocity = new Vector2(_playerRb.velocity.x, _playerJump);
                isGround = false;
                isSliding = false;
                AudioScripts.instance.playSound(jumpClip);
                _ExtraJump--;
              
            }
        }



        //FLip
        if (horizontalInput > 0 && m_facing_right)
        {
            // transform.localScale = Vector3.one;
            Flip();
        }
        else if (horizontalInput < 0 && !m_facing_right)
        {
            // transform.localScale = new Vector3(-1, 1, 1);
            Flip();
        }


        //Crouch
        if (Input.GetKeyDown(KeyCode.C) && isGround || Input.GetKeyDown(KeyCode.DownArrow) && isGround || Input.GetKeyDown(KeyCode.S) && isGround)
        {
            _crouchTrue();
        }

        if (Input.GetKey(KeyCode.C) == false && Input.GetKey(KeyCode.DownArrow) == false && Input.GetKey(KeyCode.S) == false)
        {    
            if (Physics2D.OverlapCircle(overheadCheckCollider.position, overheadCheckRadius, groundLayer))//check vật thể trên đầu
            {
                _crouchTrue();
            }
            else
            {
                _crouchFalse();
            }
        }


        //Dash
        if(Input.GetKeyDown(KeyCode.Q) && canDash)
        {
           // FindObjectOfType<HealthBar>().loseMana(10);
            StartCoroutine(Dash());
        }


        //khóa di chuyển
        bool canMove()
        {
            bool can = true;
            if (FindObjectOfType<Inventory>().isOpen || FindObjectOfType<Menu>().isMenu || isDead)
                can = false;
            return can ;
        }


    }

    //nút nhấn để dash
    public void DashButton()
    {
        StartCoroutine(Dash());
    }



    private IEnumerator Dash()
    {
        cooldowndash = false;
        isDashing = true;
        canDash = false;
        float originalGravity = _playerRb.gravityScale;
        _playerRb.gravityScale = 0f;//tắt trọng lực
        _playerRb.velocity = new Vector2(transform.right.x * dashForce, 0f);//rb nhận hướng * lực trục X
        tr.emitting = true;//hiệu ứng
        anim.Play("Dash");
        AudioScripts.instance.playSound(dashClip);
        yield return new WaitForSeconds(dashingTime);//thời gian lướt
        tr.emitting = false;
        isDashing = false;
        
        _playerRb.gravityScale = originalGravity;//trả lại trọng lực
        anim.SetBool("Dash", false);
        yield return new WaitForSeconds(isDashingColldown);//coll down time
        canDash = true;
    }

    void Flip()
    {
        m_facing_right = !m_facing_right;
        transform.Rotate(0f, 180f, 0f);
    }


    //bật collider và anim crouch
    void _crouchTrue()
    {
        crouch = true;
        crouchCollider.enabled = true;
        standingCollider.enabled = false;       
        anim.SetBool("Crouch", true);
    }
    //tắt collider và anim crouch
    void _crouchFalse()
    {
        crouch = false;
        standingCollider.enabled = true;
        crouchCollider.enabled = false;
        anim.SetBool("Crouch", false);
    }

    //bật anim slide
    void slide()
    {
        isSliding = true;
        anim.SetBool("Slide", true);
        anim.SetBool("Jump", false);
    }



    void wallCheck()
    {
        if(Physics2D.OverlapCircle(wallCheckCollider.position, wallCheckRadius, wallLayer) 
            && _playerRb.velocity.y < 0
            && !isGround)
        {
            Vector2 v = _playerRb.velocity;
            // trượt xuống nhanh hơn
            if (Input.GetKey(KeyCode.DownArrow)) v.y = -5;
            else v.y = -slideFactor;
            _playerRb.velocity = v;
            _ExtraJump = 3;
            slide();
        }
    }

    void GroundCheck()
    {
        //chân của nhân vật groundCheckCollider(GameObject CheckGround) 
        //khoang cách groundCheckRadius
        //mặt đất  groundLayer
        isGround = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position,groundCheckRadius, groundLayer);
        if(colliders.Length > 0.1f)
        {
            isGround = true;
            isSliding = false;
            _ExtraJump = 2;
            anim.SetBool("Slide", false);
        }   
        anim.SetBool("Jump", !isGround );
    }


    //Shoot mellee attack
    public void Shoot()
    {
        if (FindObjectOfType<HealthBar>().Mana >= 20 && isKame)
        {
            isKame = false;
            FindObjectOfType<HealthBar>().loseMana(20);
            Instantiate(butletPrefabs, firePoint.position, firePoint.rotation);
            AudioScripts.instance.playSound(kameClip);
        }
        else Debug.Log("Không dùng được lúc này");
    }
    public void MeleeAttack()
    {
        isAttacking = true;
        isMelle = false;
       
        //  FindObjectOfType<HealthBar>().Mana = FindObjectOfType<HealthBar>().Mana + 3;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackrange, Enemy);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy") || enemy.CompareTag("Boss"))
            {
                //Instantiate(HITEffect, enemy.GetComponent<EnemyScript>().transform.position, transform.rotation);
                enemy.GetComponent<EnemyHealBar>().takeDame(20);
            }
        }
    }



    public void Die()
    {
        isDead = true;
        FindObjectOfType<HealthBar>().DiePanel.SetActive(true);
        crouchCollider.enabled = false;
        standingCollider.enabled = false;
    }


    //hồi sinh lại người chơi
   void playerRespawn()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isDead)
        {
            isDead = false;
            FindObjectOfType<LevelManager>().Restart();
          
        }
    }
    public void buttonRepawn()
    {
        isDead = false;
        FindObjectOfType<LevelManager>().Restart();
       

    }

    void cooldownKame()
    {
        KameTime -= Time.deltaTime;

        if (KameTime <= 0)
        {
            isKame = true;
            KameTime = KameTimeLife;
        }
    }
    void cooldownMelleAttack()
    {
        MelleTime -= Time.deltaTime;

        if (MelleTime <= 0)
        {
            isMelle = true;
            MelleTime = MelleTimeLife;
        }
    }
    void cooldownDash()
    {
        DashTime -= Time.deltaTime;

        if (DashTime <= 0)
        {
            cooldowndash = true;
            DashTime = isDashingColldown;
        }
    }


    public void walkSound()
    {
        AudioScripts.instance.playSound(walkClip);
    }
    public void attackSound()
    {
        AudioScripts.instance.playSound(attackClip);
    }
   

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheckCollider.position, groundCheckRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(overheadCheckCollider.position, overheadCheckRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(wallCheckCollider.position, wallCheckRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPoint.position, attackrange);
    }

}
