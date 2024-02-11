using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocoHolding : MonoBehaviour
{
    bool isCalling = false;
    AudioSource audio;
    public float callTickDelay = 30f;
    public float callPercent = 50f;

    private void Start()
    {
        audio = transform.GetChild(0).GetComponent<AudioSource>();
        StartCoroutine(callLoop()); 
    }
    IEnumerator callLoop()
    {
        if(!isCalling)
        {
            yield return new WaitForSeconds(callTickDelay);
            if (Random.Range(0, 100) <= callPercent)
            {
                isCalling = true;
                audio.Play();
            }
            else
            {
                print("Poco not called this time.");
            }
        }
        if(!isCalling) StartCoroutine(callLoop());
    }

    public void Explode()
    {

    }
}
