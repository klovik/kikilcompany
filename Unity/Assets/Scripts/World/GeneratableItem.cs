using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratableItem : MonoBehaviour
{
    [Range(0, 100)]
    public float generatePercent = 50f;

    public bool TryGenerate()
    {
        if (Random.Range(0, 100) > generatePercent)
        {
            Destroy(gameObject);
            return false;
        }
        return true;
    }
}
