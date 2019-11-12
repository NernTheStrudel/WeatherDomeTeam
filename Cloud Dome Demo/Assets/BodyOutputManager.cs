using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyOutputManager : MonoBehaviour
{

    public BodySourceView kinectData;

    public List<GameObject> bodies;

    public GameObject handPrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //check for new bodies

        foreach(KeyValuePair<ulong, GameObject> kvp in kinectData._Bodies)
        {
            bool isNew = true;
            foreach(GameObject body in bodies)
            {
                if(body.name == kvp.Value.name)
                {
                    isNew = false;
                    break;
                }
            }

            if(isNew)
            {
                bodies.Add(kvp.Value);
                createNewBody(kvp.Value);
            }
        }


    }

    void createNewBody(GameObject bodyObject)
    {
        Transform leftHand = bodyObject.transform.Find("HandLeft");
        Transform rightHand = bodyObject.transform.Find("HandRight");

        HandFollow left =  Instantiate(handPrefab).GetComponent<HandFollow>();
        left.toFollow = leftHand;
        


        HandFollow right = Instantiate(handPrefab).GetComponent<HandFollow>();
        right.toFollow = rightHand;

    }
}
