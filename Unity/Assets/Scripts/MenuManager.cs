using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject[] screens;

    public enum Screen
    {
        None, Main, Singleplayer, Multiplayer, About
    }

    public void ChangeScreen(int _scr)
    {
        string scr = "";
        switch(_scr)
        {
            case 0: scr = "Main"; break;
            case 1: scr = "Singleplayer"; break;
            case 2: scr = "Multiplayer"; break;
            case 3: scr = "About"; break;
            case 4: scr = "Options"; break; 
        }
        for(int i = 0; i < screens.Length; i++)
        {
            if (screens[i].name != scr) screens[i].SetActive(false);
            else screens[i].SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void GotoGame()
    {
        SceneManager.LoadScene("Game");
    }
}
