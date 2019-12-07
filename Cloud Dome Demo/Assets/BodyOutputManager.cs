using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BodyOutputManager : MonoBehaviour
{
    

    public BodySourceView kinectData;

    public Dictionary<ulong, GameObject> bodies;

    public Dictionary<ulong, Vector3> bodySpeed;

    public Dictionary<ulong, GameObject> beachTowels;

    public Dictionary<ulong, Vector3[]> previousHandPos;

    public BeachBallSpawner ballSpawner;

    public Dictionary<ulong, Vector3[]> previousFootPos;

    public float kickSpeed = 1;

    public FMODUnity.StudioEventEmitter emitter;

    public float setvalue = 0.2f;

    public float towelCeiling = -3;

    public float fireworksThreshold = 3;

    public GameObject towelPrefab;

    public Vector3 TowelOffset;

    public float lanternThreshold;

    public LightLanterns[] lanterns;

    public Rotate cycleManager;

    public bool isFireworks = false;

    public FireworksSpawner fireworksSpawner;

    public float ballForce;

    public Material surferMat;
    public bool surferOn = false;

    public float surfDistance = 3;
    public float surfHeight = 1;

    public bool lifeguardWhistling = false;
    public float runSpeed = 50;
    public AudioSource whistleSound;

    // Start is called before the first frame update
    void Start()
    {
        bodies = new Dictionary<ulong, GameObject>();
        beachTowels = new Dictionary<ulong, GameObject>();
        previousHandPos = new Dictionary<ulong, Vector3[]>();
        previousFootPos = new Dictionary<ulong, Vector3[]>();
        bodySpeed = new Dictionary<ulong, Vector3>();
    }

    void doSurfer()
    {
        //if a body has hands outstretched and the surfer is not already active AND it is daytime, do surfer
        
        if(!surferOn)
        {
            //does the body have hands outstreched?
            foreach (KeyValuePair<ulong, GameObject> kvp in kinectData._Bodies)
            {
                if(surferOn)
                {
                    break;
                }
                

                Vector3 leftPos = kvp.Value.transform.Find("HandLeft").transform.position;
                Vector3 rightPos = kvp.Value.transform.Find("HandRight").transform.position;


                if (Vector3.Distance(leftPos, rightPos) > surfDistance && Mathf.Abs(leftPos.y - rightPos.y) < surfHeight)
                {
                    
                    StartCoroutine(toggleSurfer());
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //check for new bodies
        if (cycleManager.daytime)
        {

            doFireworks();


        }
        else
        {
            doSurfer();

            doKick();
        }

        //List<int> bodiesToRemove = new List<int>();
        //for(int i = 0; i < bodies.Count; i++)
        //{
        //    Debug.Log(bodies[i]);
        //    if (bodies[i] == null)
        //    {
        //        bodiesToRemove.Add(i);
        //    }
        //}

        //foreach(int i in bodiesToRemove)
        //{
        //    bodies.RemoveAt(i);
        //}

        //
        List<ulong> keysToRemove = new List<ulong>();
        foreach (KeyValuePair<ulong, GameObject> bodyKVP in bodies)
        {
            if (!kinectData._Bodies.ContainsKey(bodyKVP.Key))
            {
                keysToRemove.Add(bodyKVP.Key);
                
            }
        }
        foreach(ulong u in keysToRemove)
        {
            bodies.Remove(u);
            previousFootPos.Remove(u);
            previousHandPos.Remove(u);
            bodySpeed.Remove(u);
        }

        doBodySpeed();
        foreach (KeyValuePair<ulong, GameObject> kvp in kinectData._Bodies)
        {
            bool isNew = true;
            foreach (KeyValuePair<ulong, GameObject> bodyKVP in bodies)
            {
                //if (body == null)
                //{
                //    bodies.Remove(body);
                //    previousHandPos.Remove(kvp.Key);
                //    previousFootPos.Remove(kvp.Key);
                //}
                
                if (bodyKVP.Value.name == kvp.Value.name)
                {
                    previousHandPos[kvp.Key] = new Vector3[] { kvp.Value.transform.Find("HandLeft").transform.position, kvp.Value.transform.Find("HandRight").transform.position };
                    previousFootPos[kvp.Key] = new Vector3[] { kvp.Value.transform.Find("AnkleLeft").transform.position, kvp.Value.transform.Find("AnkleRight").transform.position };
                    isNew = false;
                    break;
                }
            }

            if (isNew)
            {
                bodies.Add(kvp.Key, kvp.Value);
                Debug.Log("NEW BODY");
                createNewBody(kvp.Value);

                previousHandPos.Add(kvp.Key, new Vector3[] { kvp.Value.transform.Find("HandLeft").transform.position, kvp.Value.transform.Find("HandRight").transform.position });
                bodySpeed.Add(kvp.Key, kvp.Value.transform.Find("SpineBase").transform.position);
                previousFootPos.Add(kvp.Key, new Vector3[] { kvp.Value.transform.Find("AnkleLeft").transform.position, kvp.Value.transform.Find("AnkleRight").transform.position });


            }
        }

        //update body speeds
        lookForBeachTowel();
        if (cycleManager.daytime)
        {
            doLanterns();
            


        }
        

    }

    void doBodySpeed()
    {

        List<KeyValuePair<ulong, Vector3>> newPos = new List<KeyValuePair<ulong, Vector3>>();

        foreach(KeyValuePair<ulong, Vector3> kvp in bodySpeed)
        {
            //get current and old position, calculate body speed
            float running = Vector3.Distance(kinectData._Bodies[kvp.Key].transform.Find("SpineBase").transform.position, kvp.Value) / Time.deltaTime;

            if(!lifeguardWhistling && running > runSpeed)
            {
                StartCoroutine(lifeGuardWhistle());
            }

            //Update position
            newPos.Add(new KeyValuePair<ulong, Vector3>(kvp.Key, kinectData._Bodies[kvp.Key].transform.Find("SpineBase").transform.position));
           // bodySpeed[kvp.Key] = kinectData._Bodies[kvp.Key].transform.Find("SpineBase").transform.position;
        }
        bodySpeed = new Dictionary<ulong, Vector3>();
        foreach(KeyValuePair<ulong, Vector3> newKVP in newPos)
        {
            bodySpeed[newKVP.Key] = newKVP.Value;
        }
    }

    public IEnumerator lifeGuardWhistle()
    {
        whistleSound.Play();
        lifeguardWhistling = true;
        yield return new WaitForSeconds(4);
        lifeguardWhistling = false;
    }

    public IEnumerator toggleSurfer()
    {
        surferOn = true;
        surferMat.DOFade(1, 1);
        yield return new WaitForSeconds(5);
        surferMat.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        surferOn = false;
    }

    void doFireworks()
    {
        //we need to get a bunch of motion from each body
        foreach (KeyValuePair<ulong, Vector3[]> kvp in previousHandPos)
        {
            if (kinectData._Bodies.ContainsKey(kvp.Key))
            {

                if (kinectData._Bodies[kvp.Key].transform.Find("HandLeft").transform.position.y > fireworksThreshold && kinectData._Bodies[kvp.Key].transform.Find("HandRight").transform.position.y > fireworksThreshold)
                {
                    if (!isFireworks)
                    {
                        Debug.Log("FIREWORKS");
                        StartCoroutine(fireworksTimer());
                        fireworksSpawner.spawnFireworks();
                    }
                    //if (kvp.Value[1].y < fireworksThreshold || kvp.Value[0].y < fireworksThreshold)
                    //{
                    //}
                }

            }
        }
    }

    public IEnumerator fireworksTimer()
    {
        isFireworks = true;
        yield return new WaitForSeconds(0.5f);
        isFireworks = false;
    }

    void doLanterns()
    {
        foreach (KeyValuePair<ulong, GameObject> kvp in kinectData._Bodies)
        {


            bool rightHandPrev = previousHandPos[kvp.Key][1].y >= lanternThreshold;
            bool leftHandPrev = previousHandPos[kvp.Key][0].y >= lanternThreshold;

            Vector3 leftPos = kvp.Value.transform.Find("HandLeft").transform.position;
            Vector3 rightPos = kvp.Value.transform.Find("HandRight").transform.position;

            bool rightHand = rightPos.y >= lanternThreshold;
            bool leftHand = leftPos.y >= lanternThreshold;

            if ((leftHand && !rightHand && leftHandPrev) || (!leftHand && rightHand && rightHandPrev))
            {
                //find the nearest lantern to the person
                Vector3 personPos = kvp.Value.transform.position;
                LightLanterns closestLantern = lanterns[0];
                float previousalpha = 0;
                foreach (LightLanterns lantern in lanterns)
                {
                    Vector3 LantPos = lantern.transform.position;

                    Vector3 RL = leftPos - rightPos;
                    Vector3 LR = rightPos - leftPos;
                    Vector3 LLant = LantPos - leftPos;
                    Vector3 RLant = LantPos - rightPos;

                    float thetaLeft = Vector3.SignedAngle(LR, LLant, Vector3.up);
                    float thetaRight = Vector3.SignedAngle(RL, RLant, Vector3.up);

                    float alphaVal = 180 - thetaLeft - thetaRight;


                    if(alphaVal > previousalpha)
                    {
                        previousalpha = alphaVal;

                        closestLantern = lantern;
                    }

                    //    if (Vector3.Distance(personPos, lantern.transform.position) < Vector3.Distance(personPos, closestLantern.transform.position))
                    //    {
                    //        closestLantern = lantern;
                    //    }
                }
                closestLantern.LightLantern();
            }


        }
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

            Vector3 towelPos = kvp.Value.transform.Find("SpineBase").transform.position + TowelOffset;

            if (beachTowels.ContainsKey(kvp.Key))
            {
                //we see if the towel needs to be toggled off
                if (!rightFoot || !leftFoot || !leftHand || !rightHand)
                {
                    GameObject.Destroy(beachTowels[kvp.Key]);
                    beachTowels.Remove(kvp.Key);
                }
            }
            else
            {


                if (rightFoot && leftFoot && rightHand && leftHand)
                {


                    GameObject newTowel = Instantiate(towelPrefab, towelPos, Quaternion.identity) as GameObject;
                    beachTowels.Add(kvp.Key, newTowel);

                }
            }
        }


    }

    void doKick()
    {
        foreach (KeyValuePair<ulong, Vector3[]> kvp in previousFootPos)
        {
            //calculate leg speed
            if (kinectData._Bodies.ContainsKey(kvp.Key))
            {
                Vector3 currentLeft = kinectData._Bodies[kvp.Key].transform.Find("AnkleLeft").transform.position;
                Vector3 currentRight = kinectData._Bodies[kvp.Key].transform.Find("AnkleRight").transform.position;

                float leftSpeed = (Vector3.Distance(currentLeft, kvp.Value[0])) / Time.deltaTime;
                float rightSpeed = (Vector3.Distance(currentRight, kvp.Value[1])) / Time.deltaTime;


                if (leftSpeed > kickSpeed || rightSpeed > kickSpeed)
                {
                    //find the nearest ball
                    if (ballSpawner.allBeachBalls.Count > 0 && ballSpawner.allBeachBalls[0] != null)
                    {
                        GameObject closestBall = ballSpawner.allBeachBalls[0].gameObject;

                        Vector3 measurePosition = currentLeft;
                        if (rightSpeed > leftSpeed)
                        {
                            measurePosition = currentRight;
                        }

                        //whats the closest ball to the faster foot?
                        foreach (BeachBall b in ballSpawner.allBeachBalls)
                        {
                            if (Vector3.Distance(b.transform.position, measurePosition) < Vector3.Distance(closestBall.transform.position, measurePosition))
                            {
                                closestBall = b.gameObject;
                            }
                        }
                        //KICK IT IN A RANDOM DIRECTION
                        Vector3 newForce = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * ballForce;

                        closestBall.GetComponent<Rigidbody>().AddForce(newForce);
                    }
                }

            }
        }
    }

    void createNewBody(GameObject bodyObject)
    {
   

        StartCoroutine(quickChange());


    }

    public IEnumerator quickChange()
    {
        emitter.SetParameter("Parameter 1", setvalue);

        yield return new WaitForSeconds(0.5f);

        emitter.SetParameter("Parameter 1", 0);
    }
}
