using System;
using UnityEngine;

public class DestroyObjectsInTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            if(other.gameObject.GetComponent<GrabbableObject>().isGrabbed)
                other.gameObject.GetComponent<GrabbableObject>().playerGrab.DestroyGrabbedObject();
            else
                other.gameObject.GetComponent<RespawnObject>().Respawn();
        }
    }
}
