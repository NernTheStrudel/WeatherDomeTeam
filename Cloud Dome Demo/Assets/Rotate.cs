using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rotate : MonoBehaviour
{

    float speed = 5;

    public Material skyMat;

    public Color daySky;

    public Color nightSky;
    

    float maxTime = 36;
    float timer = 36;

    bool daytime = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        timer -= Time.deltaTime;

        if(timer < 0)
        {
            timer = maxTime;

            if (daytime)
            {
                daytime = false;
                skyMat.DOColor(nightSky, "_Tint", maxTime);
            }
            else
            {
                daytime = true;
                skyMat.DOColor(daySky, "_Tint", maxTime);
            }
        }

        gameObject.transform.Rotate(new Vector3(speed * Time.deltaTime, 0, 0));
    }
}
