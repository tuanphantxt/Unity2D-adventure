using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    [Range (1,10)]
    public float smoothFactor;
    public Vector3 minValues, maxValue;

    private void Start()
    {
       
    }
    // Update is called once per frame
    void FixedUpdate()
    {
       Follow();
    }

    void Follow()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 boundPosition = new
            Vector3
            (
            Mathf.Clamp(targetPosition.x, minValues.x, maxValue.x),
            Mathf.Clamp(targetPosition.y, minValues.y, maxValue.y),
            Mathf.Clamp(targetPosition.z, minValues.z, maxValue.z)
            );
        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);//độ trễ lấy vị trí của camera
        transform.position = smoothPosition;
    }
 
}
