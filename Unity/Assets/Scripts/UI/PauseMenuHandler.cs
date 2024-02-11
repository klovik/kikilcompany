using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour
{
    public static GameObject pausePanel;

    private void Start()
    {
        pausePanel = transform.GetChild(0).gameObject;
    }

    public static void ChangePauseState(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        pausePanel.SetActive(state);
        Time.timeScale = state ? 0.0f : 1.0f;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyBindings.escape))
        {
            if(ConsoleHandler.consolePanel.activeSelf)
            {
                ConsoleHandler.Close();
            }
            else
            {
                ChangePauseState(!pausePanel.activeSelf);
            }
        }
    }

    public static void GotoMainMenu() => SceneManager.LoadScene("Menu");
    public static void Close() => ChangePauseState(false);
    public static void Open() => ChangePauseState(true);
}
