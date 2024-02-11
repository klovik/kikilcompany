using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class KeyBindings : MonoBehaviour
{
    [Header("PlayerInteraction")]
    [SerializeField] public static KeyCode use = KeyCode.E;
    [SerializeField] public static KeyCode drop = KeyCode.G;
    [Header("PlayerInventory")]
    [SerializeField] public static KeyCode slot1 = KeyCode.Alpha1;
    [SerializeField] public static KeyCode slot2 = KeyCode.Alpha2;
    [SerializeField] public static KeyCode slot3 = KeyCode.Alpha3;
    [Header("PauseMenuHandler")]
    [SerializeField] public static KeyCode escape = KeyCode.Escape;
    [Header("Noclip")]
    [SerializeField] public static KeyCode noclip = KeyCode.V;
    [SerializeField] public static KeyCode noclipForward = KeyCode.W;
    [SerializeField] public static KeyCode noclipLeft = KeyCode.A;
    [SerializeField] public static KeyCode noclipBack = KeyCode.S;
    [SerializeField] public static KeyCode noclipRight = KeyCode.D;
    [SerializeField] public static KeyCode noclipUp = KeyCode.Space;
    [SerializeField] public static KeyCode noclipDown = KeyCode.LeftControl;
}
