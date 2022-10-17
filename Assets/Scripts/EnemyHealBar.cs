using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealBar : MonoBehaviour
{
    [Header("fillbar")]
    public Image fillBar;
    public GameObject HeadBar;
    public GameObject deathEffect;
    public float healht;
    public float fullhealth;

    [Header("items")]
    public GameObject giftItems;
    // Start is called before the first frame update
    void Start()
    {
        healht = fullhealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (healht == fullhealth)
        {
            HeadBar.SetActive(false);
        }
        else HeadBar.SetActive(true);
    }

    //hàm nhận dame
    public void takeDame(int damage)
    {
        healht -= damage;
        fillBar.fillAmount = healht / fullhealth;
      

        if (healht <= 0)
        {
            die();
        }
    }

    /*
     
    //hàm nhận dame
    public void takeDame( int damage)
    {
        healht -= damage;
        fillBar.fillAmount = healht / fullhealth;
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("xuongAttack"))
        {
            anim.SetTrigger("Hurt");
        } 
       
        if (healht <= 0)
        {
            die();
        }
    }
     */

    //hàm die rồi sinh ra vật phẩm
    void die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        //Instantiate(giftItems, transform.position, Quaternion.identity);
        //anim.SetBool("Death", true);
        //GetComponent<Collider2D>().enabled = false;
        // this.enabled = false;
        //Destroy(gameObject);
        if(CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
        }
       

    }
}
