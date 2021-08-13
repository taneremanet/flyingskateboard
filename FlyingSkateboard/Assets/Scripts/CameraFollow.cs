using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;             // target is player here
    private Vector3 offset;              // distance between camera and the player   
    

    void Start()
    {
        //           player                           camera
        offset = target.transform.position - gameObject.transform.position;        
    }

    void LateUpdate()
    {
        Follow();
    }

    private void Follow()
    {             
        Vector3 targetPos = target.transform.position - offset;       

        Vector3 temp = targetPos;
        temp.x = 0f;
        temp.y = 7.45f;
        targetPos = temp;

        gameObject.transform.position = targetPos;        
    }
}
