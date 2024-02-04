using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Attributes")]
    public float currentSpeed = 3.5f;
    public float mouseSpeed = 5.0f;
    [Header("Keybinds")]
    public KeyCode forward = KeyCode.W;
    public KeyCode left = KeyCode.A;
    public KeyCode back = KeyCode.S;
    public KeyCode right = KeyCode.D;

    [Header("Debug")]
    public float mouseX;
    public float mouseY;
    public GameObject playerCam;
    private void Awake()
    {
        Cursor.visible = false;
    }
    private void FixedUpdate()
    {
        mouseX += Input.GetAxis("Mouse X") * Time.deltaTime * mouseSpeed;
        mouseY += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSpeed;

        if (mouseY > 90)
            mouseY = 90;
        if (mouseY < -90)
            mouseY = -90;

        if (Input.GetKey(forward))
            transform.Translate(new Vector3(0, 0, currentSpeed * Time.fixedDeltaTime));
        if (Input.GetKey(left))
            transform.Translate(new Vector3(-currentSpeed * Time.fixedDeltaTime, 0, 0));
        if (Input.GetKey(back))
            transform.Translate(new Vector3(0, 0, -currentSpeed * Time.fixedDeltaTime));
        if (Input.GetKey(right))
            transform.Translate(new Vector3(currentSpeed * Time.fixedDeltaTime, 0, 0));

        transform.rotation = Quaternion.Euler(0, mouseX, 0);
        playerCam.transform.rotation = Quaternion.Euler(-mouseY, mouseX, 0);
    }
}
