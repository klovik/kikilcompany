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
    public GameManager GM;

    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        switch(GM.gameState)
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

        leftMonitor.text = $"Destination: {GM.destination}\nLocation: {GM.currentLocation}";
        middleMonitor.text = $"Money: ${GM.money}";
        rightMonitor.text = $"GameState: {GM.gameState}";
    }

    public void UseLever()
    {
        lever.transform.GetChild(1).GetComponent<Animator>().Play("UseLever");
        if(GM.gameState == GameManager.GameState.Selecting)
        {
            StartCoroutine(StartShip());
        }
    }

    IEnumerator StartShip()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(GM.destination);
    }

    void LandShip()
    {
        string destination = GM.destination;
        SceneManager.LoadScene(destination);
    }



}
