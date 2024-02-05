using UnityEngine;
using Mirror;

public class NetworkController : NetworkBehaviour
{
    [SerializeField] private StolenPlayerController playerMovement;
    [SerializeField] private Camera playerCam;

    private void Awake()
    {
        playerMovement = GetComponent<StolenPlayerController>();
    }

    private void Start()
    {
        if (!isLocalPlayer)
        {
            playerMovement.enabled = false;
            playerCam.enabled = false;
            //playerCam.GetComponent<AudioListener>().enabled = false;
        }
    }
}