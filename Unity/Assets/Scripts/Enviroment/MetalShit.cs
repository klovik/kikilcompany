using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalShit : MonoBehaviour
{
    public bool checkingEnabled = true;
    public bool isTriggered = false;
    public GameObject lightning;
    public GameObject triggerZone;
    public Material redMat, greenMat, disabledMat;
    public GameObject audioSource;

    private void Update()
    {
        if (checkingEnabled && !isTriggered)
        {
            lightning.GetComponent<Renderer>().material = greenMat;
        }
        else if(!checkingEnabled && !isTriggered)
        {
            lightning.GetComponent<Renderer>().material = disabledMat;
        }
    }

    public void Trigger()
    {
        isTriggered = true;
        lightning.GetComponent<Renderer>().material = redMat;
        audioSource.GetComponent<AudioSource>().enabled = true;
    }
}
