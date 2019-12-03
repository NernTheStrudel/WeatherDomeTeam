using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;

public class ClatterSound : MonoBehaviour
{

    public FMODUnity.StudioEventEmitter emitter;

    public float setvalue = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(quickChange());
        }
    }

    public IEnumerator quickChange()
    {
        emitter.SetParameter("Parameter 1", setvalue);
        yield return new WaitForSeconds(0.5f);

        emitter.SetParameter("Parameter 1", 0);
    }
}
