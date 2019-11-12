using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{

    public Dictionary<GameObject, GameObject> handDisturbances;

    public GameObject hand1;
    public GameObject hand2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //look for new hands
        if(hand1 == null)
        {
            hand1 = GameObject.Find("HandLeft");
        }
        if(hand2 == null)
        {
            hand2 = GameObject.Find("HandRight");
        }
        
    }
}
