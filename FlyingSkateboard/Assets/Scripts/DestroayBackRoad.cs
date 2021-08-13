using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroayBackRoad : MonoBehaviour
{    
    void Start()
    {
        gameObject.SetActive(true);
        Invoke("DisableRoad", 2f);
    }  

    void DisableRoad()
    {
        gameObject.SetActive(false);
    }
}
