using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireworks : MonoBehaviour
{

    public GameObject fireworks1;
    public GameObject fireworks2;
    public GameObject fireworks3;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(makeFireworks());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator makeFireworks()
    {
        fireworks1.SetActive(true);
        yield return new WaitForSeconds(.7f);
        fireworks2.SetActive(true);
        yield return new WaitForSeconds(.9f);
        fireworks3.SetActive(true);

        GameObject.Destroy(fireworks1, 4);

        GameObject.Destroy(fireworks2, 4);

        GameObject.Destroy(fireworks3, 4);

        GameObject.Destroy(gameObject, 4);
    }
}
