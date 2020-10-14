using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoJumpZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            other.GetComponent<Player>().canJump = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            other.GetComponent<Player>().canJump = true;
        }
    }
}
