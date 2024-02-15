using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static T ArrayRandomChoice<T>(T[] array)
    {
        int index = Random.Range(0, array.Length);
        return array[index];
    }
}
