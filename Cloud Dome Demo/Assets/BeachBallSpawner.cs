using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachBallSpawner : MonoBehaviour
{

    public GameObject BeachBallPrefab;

    public int minimumBeachBalls;
    public int maximumBeachBalls;

    public float xSpread;
    public float zSpread;

    public float yValue = 10;

    public List<BeachBall> allBeachBalls;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBeachBalls()
    {
        allBeachBalls = new List<BeachBall>();

        //spawn a random number of beach balls and add to list

        int numberToSpawn = Random.Range(minimumBeachBalls, maximumBeachBalls);

        for(int i = 0; i < numberToSpawn; i++)
        {
            //spawn a beachball
            BeachBall newBall = Instantiate(BeachBallPrefab, new Vector3(Random.Range(xSpread * -1, xSpread), Random.Range(yValue - 3, yValue + 3), Random.Range(zSpread * -1, zSpread)), Quaternion.identity).GetComponent<BeachBall>();
            allBeachBalls.Add(newBall);
        }
    }

    public void DestroyBeachBalls()
    {
        foreach(BeachBall b in allBeachBalls)
        {
            b.DestroySelf();
        }

        allBeachBalls = new List<BeachBall>();

    }
}
