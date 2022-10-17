using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTime : MonoBehaviour
{
    public float Time =1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, Time);
    }

   
}
