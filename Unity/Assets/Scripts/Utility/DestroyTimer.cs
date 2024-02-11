using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public int destroySeconds = 10;
    void Start()
    {
        StartCoroutine(destroyTimer(destroySeconds));
    }
    IEnumerator destroyTimer(int seconds)
    {
        if (gameObject.GetComponent<ParticleSystem>() != null)
        {
            yield return new WaitForSeconds(seconds / 2);
            gameObject.GetComponent<ParticleSystem>().Stop();
            yield return new WaitForSeconds(seconds / 2);
            Destroy(gameObject);
        }
        else
        {
            yield return new WaitForSeconds(seconds);
            Destroy(gameObject);
        }
    }
}
