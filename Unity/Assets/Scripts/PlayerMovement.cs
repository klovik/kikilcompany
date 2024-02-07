using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public bool canMove = true;

    [Header("Running")]
    public bool canRun = true;
    public bool noclip = false;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;
    public GameObject console;
    public GameObject pauseMenu;

    Rigidbody rb;
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (canMove && !noclip)
        {
            IsRunning = canRun && Input.GetKey(runningKey);
            float targetMovingSpeed = IsRunning ? runSpeed : speed;
            if (speedOverrides.Count > 0)
                targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
            Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);
            rb.velocity = transform.rotation * new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.y);
        }
        else if(canMove && noclip)
        {
            if(Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right);
            }
            if(Input.GetKey(KeyCode.Space))
            {
                transform.Translate(Vector3.up);
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                transform.Translate(Vector3.down);
            }
        }
    }

    void changeNoclipState(bool state)
    {
        rb.constraints = state ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.FreezeRotationZ;
        noclip = state;
        GetComponent<CapsuleCollider>().enabled = !state;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tilde))
        {
            if (console.activeSelf)
            {
                console.SetActive(false);
                ChangePlayerMovementAbility(true);
            }
            else if (pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
                ChangePlayerMovementAbility(true);
            }
            else
            {
                pauseMenu.SetActive(true);
                ChangePlayerMovementAbility(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.V) && console.GetComponent<Terminal>().debugging)
        {
            changeNoclipState(!noclip);
        }
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        ChangePlayerMovementAbility(true);
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ChangePlayerMovementAbility(bool state)
    {
        canMove = state;
        gameObject.transform.GetChild(0).GetComponent<PlayerLook>().canMoveMouse = state;
        rb.freezeRotation = !state;
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
