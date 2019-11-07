using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{

    

    public List<GameObject> cloudObjects; // this is the datastructure that's actually USED for functionality

    public GameObject[] clouds; //this is a placeholder to show functionality

    public int activeCloud = 0;

    public float speed = 5;

    public float maxXY = 25;

    public ParticleSystem rainstorm;
    public int rainIntensityMin = 5;
    public int rainIntensityMax = 505;

    public GameObject[] lightnings;

    // Start is called before the first frame update
    void Start()
    {
        cloudObjects = new List<GameObject>();

        foreach (GameObject cloud in clouds)
        {
            cloudObjects.Add(cloud);
        }

        rainstorm.emissionRate = rainIntensityMin;
    }

    // Update is called once per frame
    void Update()
    {
        


        setRainIntensity();

        if(Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(lightning());
        }

    }

    void setRainIntensity()
    {
        float percent = 0;

        int count = 0;

        foreach(GameObject cloud in cloudObjects)
        {
            if (cloud.transform.position.y < 0)
            {
                count++;
            }
        }

        percent = (count * 1.0f) / cloudObjects.Count;

        int emitRate = (int)(percent * (rainIntensityMax - rainIntensityMin));
        emitRate += rainIntensityMin;

        rainstorm.emissionRate = emitRate;

    }

    public IEnumerator lightning()
    {
        int i = Random.Range(0, lightnings.Length - 1);

        lightnings[i].SetActive(true);

        yield return new WaitForSeconds(.2f);

        lightnings[i].SetActive(false);

    }
}
