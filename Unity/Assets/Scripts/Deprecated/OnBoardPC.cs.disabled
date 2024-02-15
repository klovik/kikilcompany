using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnBoardPC : MonoBehaviour
{
    public Text leftMonitor, middleMonitor, rightMonitor;
    public GameObject lever;

    void Update()
    {
        switch(GameManager.gameState)
        {
            case GameManager.GameState.None:
                break;
            case GameManager.GameState.Playing:
                lever.GetComponent<Label>().text = "Start ship";
                break;
            case GameManager.GameState.Landing:
                lever.GetComponent<Label>().text = "Land ship";
                break;
        }

        leftMonitor.text = $"Destination: {GameManager.destination}\nLocation: {GameManager.currentLocation}";
        middleMonitor.text = $"Money: ${GameManager.money}";
        rightMonitor.text = $"GameState: {GameManager.gameState}";
    }

    public void UseLever()
    {
        lever.transform.GetChild(1).GetComponent<Animator>().Play("UseLever");
        if(GameManager.gameState == GameManager.GameState.Selecting)
        {
            StartCoroutine(StartShip());
        }
    }

    IEnumerator StartShip()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(GameManager.destination);
    }

    void LandShip()
    {
        string destination = GameManager.destination;
        SceneManager.LoadScene(destination);
    }



}
