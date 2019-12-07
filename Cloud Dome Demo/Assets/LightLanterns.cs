using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLanterns : MonoBehaviour {

    public GameObject lanternFire;

    public GameObject currentFire;

    public Transform fireRoot;

    public bool alreadyLit = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnMouseDown()
    {
        LightLantern();
    }

    public void LightLantern()
    {
        if (!alreadyLit)
        {
            if (currentFire == null)//light it
            {
                currentFire = Instantiate(lanternFire, fireRoot.position, new Quaternion(0, 0, 0, 0), fireRoot);

            }
            else // unlight it
            {
                Destroy(currentFire);
            }
            StartCoroutine(lanternTimer());
        }


    }

    public IEnumerator lanternTimer()
    {
        alreadyLit = true;
        yield return new WaitForSeconds(1);
        alreadyLit = false;
    }
}
