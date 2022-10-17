using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatChase : MonoBehaviour
{
    public BatEnemyScript[] BatArray;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           foreach(BatEnemyScript enemy in BatArray)
            {
                enemy.chase = true;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (BatEnemyScript enemy in BatArray)
            {
                enemy.chase = false;
            }
        }
    }
}
