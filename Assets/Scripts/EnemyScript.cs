using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{

    [Header("Attack")]
    #region Public Variables
    public Transform rayCast;
    public LayerMask raycastMask;
    public float rayCastLength;
    public float attackDistance; //khoảng cách tối thiểu để tấn công
    public float moveSpeed;
    public float timercooldame; //thời gian hồi lại đòn đánh
    public Transform right_Limit;
    public Transform left_Limit;
    public GameObject Fireattack;
    public Transform target;//đối tượng hướng đến
    public GameObject hitBox;
    #endregion

    #region Private Variables
    private RaycastHit2D hit;//lưu trữ thông tin raycast
    
    private Animator anim;
    private float distance; //khoảng cách giữa enemy và player
    private bool attackMode;//tấn công 
    private bool inRange; //kiểm tra người chơi trong phạm vi
    private bool cooling; //kiểm tra kẻ thù sẽ bỏ cuộc sau khi mất phạm vi
    private float intTimer;//lưu trữ thời gian sau
    #endregion


    [Header("music")]
    public AudioClip inrangeClip, attackClip;


    void Awake()
    {
      
    }

     void Start()
    {
        SelectTargets();
        intTimer = timercooldame; //thời gian tấn công ban đầu 
        anim = GetComponent<Animator>();
       
    }
     void Update()
    {
        hitboxdame();

        if (!attackMode )
        {
            Move();
        }
        if(!insideofLimits ()&& !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("xuongAttack"))
        {
            SelectTargets();
        }

        if (inRange)
        {  
            //hit là 1 đường chạy ra từ vị trí của raycast để bắt tượng
             hit = Physics2D.Raycast(rayCast.position, transform.right, rayCastLength, raycastMask);
             RaycastDebugger();
             Fireattack.SetActive(true);
        }

        //Khi enemy phát hiện đc người chơi
        if (hit.collider != null)
        {
            EnemyLogic();
        }
        else if (hit.collider == null) //khi không phát hiện đc đối tượng nữa
        {       
            StopAttack();
        }
    }


   


    //phát hiện player khi vào Area
    //phát hiện trả inRange = true
    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            target = trig.transform;//nhận đối tượng dể di chuyển
            inRange = true;
            Flip();
            Debug.Log("Đã thấy đối tượng");
            //AudioScripts.instance.playSound(inrangeClip);
        }
    }
    void OnTriggerExit2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            StopAttack();
            Fireattack.SetActive(false);
            Flip();
            inRange = false;
        }
    }


    //dùng để xem đường chỉ raycast trong Scene
    void RaycastDebugger()
    {
        if (distance > attackDistance)//khoảng cách giữa enemy và play > khoảng cách tối thiểu tấn công
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.red);
        }
        else if (attackDistance > distance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.green);
        }
    }


    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);//nhận khoảng cách giữa nó và vị trí targets(player)

        if (distance > attackDistance)
        {
            StopAttack();
        }
        else if (attackDistance >= distance && cooling == false)
        {
            Attack();
        }

        if (cooling)
        {
            Cooldown();
            anim.SetBool("Attack", false);
        }
    }
    //tấn công theo thời gian
    //bỏ hành động walk
    //bật hành động tấn  công
    void Attack()
    {
        intTimer = timercooldame ; //Đặt lại bộ đếm thời gian khi người chơi nhập phạm vi tấn công
        attackMode = true; //Để kiểm tra xem kẻ thù có còn tấn công hay không

        anim.SetBool("canWalk", false);
        anim.SetBool("Attack", true);
    }

    //ngưng tấn công 
    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("Attack", false);
    }

    //hàm di chuyển
    void Move()
    {
        anim.SetBool("canWalk", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("xuongAttack")) //nếu đang không thực hiện hành động này
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            // di chuyển sprite đến vị trí đích 
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (inRange) transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed*1.5f * Time.deltaTime);
        }
    }

    //duoc95 gọi trong animation
    public void TriggerCooling()
    {
        cooling = true;
    }


    void Cooldown()
    {
        intTimer -= Time.deltaTime;

        if (intTimer <= 0 && cooling && attackMode)
        {
            cooling = false;
            intTimer = timercooldame;
        }
    }

    //hàm giới hạn di chuyển 
    private bool insideofLimits()
    {
        return transform.position.x > left_Limit.position.x && transform.position.x < right_Limit.position.x;
    }


    //hàm chọn đối tượng
    private void SelectTargets()
    {
        float distanceToleft = Vector2.Distance(transform.position, left_Limit.position);
        float distanceToRight = Vector2.Distance(transform.position, right_Limit.position);

        if (distanceToleft > distanceToRight)
        {
            target = left_Limit;
        }
        else 
        {
            target = right_Limit; 
        }
        Flip();
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

    void hitboxdame()
    {
        hitBox.SetActive(attackMode);
    }
    void soundAttack()
    {
        AudioScripts.instance.playSound(attackClip);
    }

}

