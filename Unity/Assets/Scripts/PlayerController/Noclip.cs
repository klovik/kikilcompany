using Fragsurf.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Noclip : MonoBehaviour
{
    private static bool noclipping = false;
    private static SurfCharacter sc;
    private float noclipSpeed = .5f;
    [SerializeField] private static Text noclipText;
    public float baseNoclipSpeed = .1f;

    private void Start()
    {
        sc = GetComponent<SurfCharacter>();
        noclipText = GameObject.Find("NoclipText").GetComponent<Text>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyBindings.noclip) && GameManager.developer)
        {
            ChangeNoclipState(!noclipping);
        }

        if (noclipping)
        {
            noclipText.text = $"noclip speed: {noclipSpeed}";

            transform.rotation = transform.GetChild(1).transform.rotation;

            if (Input.GetAxis("Mouse ScrollWheel") > 0f) noclipSpeed += noclipSpeed != 10 ? .1f : 0f;
            if (Input.GetAxis("Mouse ScrollWheel") < 0f) noclipSpeed -= noclipSpeed != 0 ? .1f : 0f;

            if (Input.GetKey(KeyBindings.noclipForward))
            {
                transform.Translate(Vector3.forward * noclipSpeed);
            }
            if (Input.GetKey(KeyBindings.noclipBack))
            {
                transform.Translate(Vector3.back * noclipSpeed);
            }
            if (Input.GetKey(KeyBindings.noclipLeft))
            {
                transform.Translate(Vector3.left * noclipSpeed);
            }
            if (Input.GetKey(KeyBindings.noclipRight))
            {
                transform.Translate(Vector3.right * noclipSpeed);
            }
            if (Input.GetKey(KeyBindings.noclipUp))
            {
                transform.Translate(Vector3.up * noclipSpeed);
            }
            if (Input.GetKey(KeyBindings.noclipDown))
            {
                transform.Translate(Vector3.down * noclipSpeed);
            }
        }
    }

    public void ChangeNoclipState(bool state)
    {
        noclipSpeed = baseNoclipSpeed;
        sc.enabled = !state;
        noclipping = state;
        if (!state)
        {
            noclipText.text = "";
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
    }
    public void On() => ChangeNoclipState(true);
    public void Off() => ChangeNoclipState(false);
}
