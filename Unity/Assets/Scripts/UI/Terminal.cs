using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Terminal : MonoBehaviour
{
    //terminal fits 16 string with 82 chars each
    public InputField textField;
    public Text consoleText;
    private GameObject player;
    public bool dev = false;
    public bool debugging = false;
    private List<string> lines = new List<string>();
    public GameManager GM;
    public string dest = "company";
    public string toConfirmDest = "";

    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        if(Debug.isDebugBuild || Application.isEditor)
        {
            dev = true; debugging = true;
        }
    }

    public void OnEndEdit()
    {
        string text = textField.text;
        textField.text = "";
        Print($"> {text}");
        //SendCommand(text);
        textField.ActivateInputField();
    }
    // void SendCommand(string text)
    // {
    //     switch(text.Split(' ')[0].ToLower())
    //     {
    //         case "help":
    //             Print("Command list:");
    //             Print("help - Get help");
    //             Print("close - Close console");
    //             Print("clear - Clear console");
    //             if(debugging)
    //             {
    //                 Print("Debug Commands:");
    //                 Print("debugging on/off - Toggle debug mode");
    //                 Print("scene <name> - Load scene/Get active scene");
    //                 Print("scenes - Get list of all scenes");
    //                 Print("indexing - Count from 0 to 100");
    //             }
    //             break;
    //         case "close":
    //             ConsoleHandler.Close();
    //             Print("Closing..");
    //             break;
    //         case "clear":
    //         case "cls":
    //             consoleText.text = ""; lines.Clear();
    //             break;
    //         case "debugging":
    //             try
    //             {
    //                 if (dev)
    //                 {
    //                     switch (text.Split(' ')[1])
    //                     {
    //                         case "on":
    //                             debugging = true;
    //                             Print("Debugging is on!");
    //                             break;
    //                         case "off":
    //                             debugging = false;
    //                             Print("Debugging is off!");
    //                             break;
    //                         default:
    //                             Print(Error.Construction);
    //                             break;
    //                     }
    //                 }
    //                 else
    //                 {
    //                     Print(Error.NotAnDev); break;
    //                 }
    //                 break;
    //             }
    //             catch
    //             {
    //                 Print(Error.Failed);
    //             }
    //             break;
    //         case "moons":
    //         case "moonlist":
    //             for(int i = 0; i < GameManager.moons.Length; i++)
    //             {
    //                 Print(GameManager.moons[i]);
    //             }
    //             break;
    //         case "destination":
    //         case "dest":
    //         case "d":
    //             if(text.Split(' ').Length == 1)
    //             {
    //                 Print($"Current destination is: {GameManager.destination}");
    //                 break;
    //             }
    //             else if (text.Split(' ').Length == 2)
    //             {
    //                 bool moonExist = false;
    //                 for(int i = 0; i < GameManager.moons.Length; i++)
    //                 {
    //                     if (GameManager.moons[i] == text.Split(' ')[1])
    //                     {
    //                         moonExist = true; break;
    //                     }
    //                 }
    //                 if(moonExist)
    //                 {
    //                     Print($"Set destination to {text.Split(' ')[1].ToLower()}? Type 'confirm' to accept.");
    //                     toConfirmDest = text.Split(' ')[1].ToLower();
    //                     break;
    //                 }
    //                 else
    //                 {
    //                     Print("Moon not found!");
    //                     break;
    //                 }
    //             }
    //             else
    //             {
    //                 Print(Error.Construction);
    //                 break;
    //             }
    //         case "confirm":
    //             try
    //             {
    //                 if(toConfirmDest == "")
    //                 {
    //                     Print("There is nothing to confirm.");
    //                     break;
    //                 }
    //                 else
    //                 {
    //                     GameManager.destination = toConfirmDest;
    //                     toConfirmDest = "";
    //                     Print($"Destination successfully set to {dest}.");
    //                     break;
    //                 }
    //             }
    //             catch
    //             {
    //                 Print(Error.Failed);
    //                 break;
    //             }
    //         case "indexing":
    //             if (debugging)
    //                 for (int i = 0; i <= 100; i++)
    //                     Print($"#{i} .....");
    //             else
    //                 print(Error.NotAnDev);
    //             break;
    //         default:
    //             Print(Error.Unknown);
    //             break;
    //     }
    // }
    void Print(string text)
    {
        if(lines.Count == 21)
            lines.RemoveAt(0);

        //if(text.Split('\n').Length > 1)
        //{
        //    for(int i = 0; i < text.Split('\n').Length; i++)
        //    {
        //        Print(text.Split('\n')[i]);
        //    }
        //}

        lines.Add(text);
        consoleText.text = "";
        for(int i = 0; i < lines.Count; i++)
        {
            consoleText.text += lines[i] + "\n";
        }
    }

    void Print(Error err)
    {
        switch(err)
        {
            case Error.None:
                Print("Unknown error!"); break;
            case Error.Unknown:
                Print("Unknown command!"); break;
            case Error.Construction:
                Print("Invalid arguments!"); break;
            case Error.NotAnDev:
                Print("Not an dev build!"); break;
            case Error.Failed:
                Print("Command failed to run!"); break;
            default:
                Print("Unknown error type!"); break;
        }
    }

    enum Error
    {
        None, Unknown, Construction, NotAnDev, Failed
    }
}
