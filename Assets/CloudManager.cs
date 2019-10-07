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
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            activeCloud = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeCloud = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activeCloud = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activeCloud = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            activeCloud = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            activeCloud = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            activeCloud = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            activeCloud = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            activeCloud = 8;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            activeCloud = 9;
        }


        //movement

        if(Input.GetKey(KeyCode.UpArrow) && (clouds[activeCloud].transform.position + Vector3.up * speed * Time.deltaTime).y <= maxXY)
        {
            clouds[activeCloud].transform.position += Vector3.up * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow) && (clouds[activeCloud].transform.position + Vector3.up * speed * Time.deltaTime).y >= -1 *maxXY)
        {
            clouds[activeCloud].transform.position -= Vector3.up * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow) && (clouds[activeCloud].transform.position -= Vector3.right * speed * Time.deltaTime).x >=  -1*maxXY)
        {
            clouds[activeCloud].transform.position -= Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow) && (clouds[activeCloud].transform.position + Vector3.right * speed * Time.deltaTime).x <= maxXY)
        {
            clouds[activeCloud].transform.position += Vector3.right * speed * Time.deltaTime;
        }

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
