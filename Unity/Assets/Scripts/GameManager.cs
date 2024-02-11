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

    public enum GameState
    {
        None, Selecting, Landing, Playing
    }
}
