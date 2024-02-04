using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerNetworking : NetworkBehaviour
{
    void Start()
    {
        if (!isLocalPlayer) Destroy(transform.FindChild("Camera"));
    }
}
