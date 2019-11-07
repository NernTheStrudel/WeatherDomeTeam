using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThunderEffect : MonoBehaviour
{

    public float floor = 0.9f;
    public float ceiling = 1.5f;
    public float fadeTime = 2f;
    public float step = 0;
    public float intensity;
    public bool activated = false;
    public Color baseColor;

    public Material bckMat;

    // Start is called before the first frame update
    void Start()
    {
        baseColor = bckMat.GetColor("_EmissionColor");
        step = (ceiling - floor) / fadeTime;
        intensity = floor;
        bckMat.SetColor("_EmissionColor", baseColor * intensity);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L) && !activated)
        {
            doThunder();
        }
    }

    public void doThunder()
    {
        StartCoroutine(ThunderCoroutine());
    }

    public IEnumerator ThunderCoroutine()
    {
        activated = true;

        bckMat.DOColor(baseColor * ceiling, "_EmissionColor", fadeTime);
        yield return new WaitForSeconds(fadeTime);
        yield return new WaitForSeconds(.2f);


        bckMat.DOColor(baseColor * floor, "_EmissionColor", fadeTime);
        yield return new WaitForSeconds(fadeTime);

        activated = false;
    }
}
