using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class huongdan : MonoBehaviour
{

    public GameObject thongbao;
    public bool isthongbao;

    private void Update()
    {
        
    }
    public  void thongtin()
    {
        isthongbao = !isthongbao;
        thongbao.SetActive(isthongbao);
    }

     void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            thongtin();
        }
    }
     void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            thongtin();
        }
    }
}
