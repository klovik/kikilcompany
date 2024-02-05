using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public bool canMove = true;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    Rigidbody rb;
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            IsRunning = canRun && Input.GetKey(runningKey);
            float targetMovingSpeed = IsRunning ? runSpeed : speed;
            if (speedOverrides.Count > 0)
                targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
            Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);
            rb.velocity = transform.rotation * new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.y);
        }
    }

    public void ChangePlayerMovementAbility(bool state)
    {
        canMove = state;
        gameObject.transform.GetChild(0).GetComponent<PlayerLook>().canMoveMouse = state;
        rb.freezeRotation = !state;
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
    }
}