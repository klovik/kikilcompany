using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool developer = false;
    private Text fpstext;
    private Text velocityText;
    private GameObject player;
    private Vector3 lastPosition = Vector3.zero;

    [Header("Settings")]
    public static bool fpsCounterEnabled = true;
    public static bool velocityCounterEnabled = true;
    public static bool vSync = true;

    private enum DayTime
    {
        None, Day, Night
    }
    private void Awake()
    {
        if (Application.isEditor || Debug.isDebugBuild) developer = true;
    }

    private void Start()
    {
        if (vSync) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;
        player = GameObject.Find("Player");
        fpstext = GameObject.Find("FPS").GetComponent<Text>();
        velocityText = GameObject.Find("VelocityText").GetComponent<Text>();
    }
    private void Update()
    {
        if (!fpsCounterEnabled && fpstext.gameObject.activeSelf) fpstext.gameObject.SetActive(false);
        else fpstext.text = "FPS: " + (Mathf.FloorToInt(1.0f / Time.deltaTime)).ToString();

        if (!velocityCounterEnabled && velocityText.gameObject.activeSelf) velocityText.gameObject.SetActive(false);
        else
        {
            float speed = Vector3.Distance(player.transform.position, lastPosition) / Time.deltaTime;
            lastPosition = player.transform.position;
            velocityText.text = Mathf.Floor(speed).ToString();
        }
    }
}
