using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        HealthBar damePlayer = hitInfo.GetComponent<HealthBar>();
        if (damePlayer != null)
        {
            damePlayer.loseHeadth(10);
        }
       
        
    }
}
