using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendObjectPosition : MonoBehaviour
{
    public int objectId = 1;
    public ObjectType objectTypeType;
    
    private Vector3 lastPos = Vector3.zero;

    private void Update()
    {
        Vector3 pos = transform.position;

        if (lastPos != pos)
            ServerSend.ObjectPosition(objectTypeType, objectId, pos);

        lastPos = pos;
    }
}
