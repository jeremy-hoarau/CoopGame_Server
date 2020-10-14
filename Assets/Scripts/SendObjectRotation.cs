using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendObjectRotation : MonoBehaviour
{
    public int objectId = 1;
    public ObjectType objectTypeType;
    
    private Quaternion lastRotation = Quaternion.identity;

    private void Update()
    {
        Quaternion rotation = transform.rotation;

        if (lastRotation != rotation)
            ServerSend.ObjectRotation(objectTypeType, objectId, rotation);

        lastRotation = rotation;
    }
}
