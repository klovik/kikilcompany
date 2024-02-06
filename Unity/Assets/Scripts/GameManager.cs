using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int money = 200;
    public GameState gameState = GameState.Selecting;
    public string[] moons;
    public string destination;
    public string currentLocation;

    public enum GameState
    {
        None, Selecting, Landing, Playing
    }
}
