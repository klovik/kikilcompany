using Fragsurf.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleHandler : MonoBehaviour
{
    public static GameObject consolePanel;

    private void Start()
    {
        consolePanel = transform.GetChild(0).gameObject;
        print(consolePanel.name);
    }

    public static void ChangeConsoleState(bool state)
    {
        consolePanel.SetActive(!consolePanel.activeSelf);
        SurfCharacter.movementEnabled = !state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public static void Open() => ChangeConsoleState(true);
    public static void Close() => ChangeConsoleState(false);
}
