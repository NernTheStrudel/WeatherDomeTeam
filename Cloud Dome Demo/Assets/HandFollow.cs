using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFollow : MonoBehaviour
{

    public Transform toFollow;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(toFollow != null)
        {
            transform.position = new Vector3(toFollow.position.x, toFollow.position.y, transform.position.z);
        }
    }
}
