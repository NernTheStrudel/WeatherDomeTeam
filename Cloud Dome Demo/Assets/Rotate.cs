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

    public Material waterMat;

    public BeachBallSpawner ballSpawner;

    public float waterspeed = .2f;

    float maxTime = 36;
    float timer = 7;

    public bool daytime = false;

    bool ballsReleased = false;
    

    // Start is called before the first frame update
    void Start()
    {

        RenderSettings.skybox.DOColor(daySky, "_Tint", 1);
    }

    // Update is called once per frame
    void Update()
    {

        timer -= Time.deltaTime;
        float newVal = waterspeed * Time.deltaTime;
        waterMat.mainTextureOffset += new Vector2(newVal, newVal);

        

        if(timer < 0)
        {
            timer = maxTime;

            if (daytime)
            {
                daytime = false;
                //  skyMat.DOFloat(.02f, "_Exposure", maxTime);

                RenderSettings.skybox.DOColor(daySky, "_Tint", 5);
                if(!ballsReleased)
                {
                    ballSpawner.SpawnBeachBalls();
                    ballsReleased = true;

                
                }
            }
            else
            {
                daytime = true;
                if (!ballsReleased)
                {
                    ballSpawner.DestroyBeachBalls();

                    ballsReleased = true;
                }
                //skyMat.DOFloat(.27f, "_Exposure", maxTime);
                //RenderSettings.skybox.DOColor(daySky, 1);
                // skyMat.color = (new Color(1, 1, 1, 1));
                RenderSettings.skybox.DOColor(nightSky, "_Tint", 5);
            }

            ballsReleased = false;
        }

        gameObject.transform.Rotate(new Vector3(speed * Time.deltaTime, 0, 0));
    }

}
