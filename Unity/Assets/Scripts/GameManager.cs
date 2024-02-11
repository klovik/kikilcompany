using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int money = 200;
    public static GameState gameState = GameState.Selecting;
    public static string[] moons;
    public static string destination;
    public static string currentLocation;
    public static bool developer = false;

    private void Start()
    {
        if (Application.isEditor || Debug.isDebugBuild) developer = true;
    }

    public enum GameState
    {
        None, Selecting, Landing, Playing
    }
}
