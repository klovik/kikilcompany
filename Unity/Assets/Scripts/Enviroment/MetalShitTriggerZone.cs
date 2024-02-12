using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalShitTriggerZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && transform.parent.GetComponent<MetalShit>().checkingEnabled)
        {
            transform.parent.GetComponent<MetalShit>().Trigger();
        }
    }

    private bool isInventoryFull()
    {
        for(int i = 0; i < PlayerInventory.inventory.Length; i++)
        {
            if (PlayerInventory.inventory[i] == PlayerInventory.ItemId.None)
            {
                return false;
            }
        }
        return true;
    }
}
