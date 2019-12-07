using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworksSpawner : MonoBehaviour
{

    public GameObject fireworksPrefab;

   

    // Update is called once per frame
    void Update()
    {
     if(Input.GetKeyDown(KeyCode.F))
        {
            spawnFireworks();
        }
    }

    public void spawnFireworks()
    {
        Instantiate(fireworksPrefab, transform);
    }
}
