using System;
using UnityEngine;

public class DestroyObjectsInTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            if(other.gameObject.GetComponent<GrabbableObject>().playerGrab != null)
                other.gameObject.GetComponent<GrabbableObject>().playerGrab.DestroyGrabbedObject();
            else
                Destroy(other.gameObject);
            ServerSend.DestroyObject(ObjectType.box, other.GetComponent<SendObjectPosition>().objectId);
        }
    }
}
