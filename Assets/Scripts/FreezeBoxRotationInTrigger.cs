using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBoxRotationInTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            other.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            other.gameObject.GetComponent<Rigidbody>().freezeRotation = false;
        }
    }
}
