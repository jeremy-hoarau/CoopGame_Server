using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnePlayerInTrigger : MonoBehaviour
{
    [SerializeField][Range(1, 2)] int sensiblePlayerId = 1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger)
            return;
        switch (sensiblePlayerId)
        {
            case 1 when other.CompareTag("Player1"):
            case 2 when other.CompareTag("Player2"):
                other.gameObject.GetComponent<PlayerDeathRespawn>().Die();
                break;
        }
    }
}
