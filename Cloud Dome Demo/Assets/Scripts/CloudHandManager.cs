using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudHandManager : MonoBehaviour
{


    public Dictionary<GameObject, GameObject> cloudClass;

    public GameObject[] clouds;

    GameObject leftOne;
    GameObject rightOne;

    public float speed = 5;
    public float tolerance = 1;

    public float minSize = .3f;
    public float maxSize = 2.3f;

    public float multiplier = .5f;

    public float handFloor = 4;
    public float handCeiling = 8;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (leftOne == null)
        {
            leftOne = GameObject.Find("HandLeft");
        }
        if (rightOne == null)
        {
            rightOne = GameObject.Find("HandRight");
        }

        if(leftOne != null && rightOne != null)
        {
            Vector3 target = new Vector3((leftOne.transform.position.x + rightOne.transform.position.x)/2, (leftOne.transform.position.y + rightOne.transform.position.y)/2, clouds[0].transform.position.z);
         
            if (Vector3.Distance(clouds[0].transform.position, target) > tolerance)
            {
                clouds[0].transform.position = Vector3.MoveTowards(clouds[0].transform.position, target, speed * Time.deltaTime);
            }

            //change size
            ParticleSystem ps = clouds[0].GetComponent<ParticleSystem>();
            //var sphere = ps.shape;
            Debug.Log((Vector3.Distance(leftOne.transform.position, rightOne.transform.position)));

            float scale = ((Vector3.Distance(leftOne.transform.position, rightOne.transform.position)) - handFloor) * .5f;
            Debug.Log(scale);
            clouds[0].transform.localScale = new Vector3(scale, scale, scale);
           // sphere.radius = minSize + (Vector3.Distance(leftOne.transform.position, rightOne.transform.position));

        }
        //else do nothing
        
    }
}
