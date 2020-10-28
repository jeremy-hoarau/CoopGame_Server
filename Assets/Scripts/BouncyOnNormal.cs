using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyOnNormal : MonoBehaviour
{
    public bool removeCurrentVelocity;
    
    public float force = 950f;
    private void OnCollisionEnter(Collision other)
    {
        if (removeCurrentVelocity)
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        other.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward  * force, ForceMode.Impulse);
    }
}
