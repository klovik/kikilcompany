using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            print(hit.collider.name);
        }
        Debug.DrawRay(transform.position, ray.direction, Color.yellow);
    }
}
