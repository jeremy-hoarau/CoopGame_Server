using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityJump : MonoBehaviour
{
    public float fallMultiplier = 2.5f;

    private Rigidbody rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * (Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
        }
    }
}
