using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyOutputManager : MonoBehaviour
{

    public BodySourceView kinectData;

    public List<GameObject> bodies;

    public GameObject handPrefab;

    public Dictionary<ulong, GameObject> beachTowels;

    public FMODUnity.StudioEventEmitter emitter;

    public float setvalue = 0.2f;

    public float towelCeiling = -3;

    public GameObject towelPrefab;

    public Vector3 TowelOffset;

    // Start is called before the first frame update
    void Start()
    {
        beachTowels = new Dictionary<ulong, GameObject>();
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
                if(body == null)
                {
                    bodies.Remove(body);
                }
                else if(body.name == kvp.Value.name)
                {
                    isNew = false;
                    break;
                }
            }

            if(isNew)
            {
                bodies.Add(kvp.Value);
                createNewBody(kvp.Value);
                Debug.Log("New Body");
            }
        }

        lookForBeachTowel();


    }

    void lookForBeachTowel()
    {
        //get hands/feet

        foreach (KeyValuePair<ulong, GameObject> kvp in kinectData._Bodies)
        {

            //if all hands/feet are below threshold, make a new towel
            bool rightFoot = kvp.Value.transform.Find("AnkleRight").transform.position.y <= towelCeiling;
            bool leftFoot = kvp.Value.transform.Find("AnkleLeft").transform.position.y <= towelCeiling;
            bool rightHand = kvp.Value.transform.Find("HandRight").transform.position.y <= towelCeiling;
            bool leftHand = kvp.Value.transform.Find("HandLeft").transform.position.y <= towelCeiling;

            if (beachTowels.ContainsKey(kvp.Key))
            {
                //we see if the towel needs to be toggled off
                if(!rightFoot || !leftFoot || !leftHand || !rightHand)
                {
                    GameObject.Destroy(beachTowels[kvp.Key]);
                    beachTowels.Remove(kvp.Key);
                }
            }
            else
            {
          

                if(rightFoot && leftFoot && rightHand && leftHand)
                {


                    GameObject newTowel = Instantiate(towelPrefab, new Vector3(kvp.Value.transform.position.x + TowelOffset.x, kvp.Value.transform.position.y + TowelOffset.y, kvp.Value.transform.position.z + TowelOffset.z), Quaternion.identity) as GameObject;
                    beachTowels.Add(kvp.Key, newTowel);

                }
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

        
        StartCoroutine(quickChange());
        

    }

    public IEnumerator quickChange()
    {
        emitter.SetParameter("Parameter 1", setvalue);
        Debug.Log(emitter.Params[0]);
        yield return new WaitForSeconds(0.5f);

        emitter.SetParameter("Parameter 1", 0);
    }
}
