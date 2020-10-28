using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    public Transform respawnPoint_Player1, respawnPoint_Player2;
    public UnityEvent onCheckpointActivated;
    
    private bool player1Arrived, player2Arrived;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            player1Arrived = true;
        }
        else if(other.CompareTag("Player2"))
        {
            player2Arrived = true;
        }
        
        if (player1Arrived && player2Arrived)
        {
            SetCheckpointForAllPlayers();
            onCheckpointActivated.Invoke();
            enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            player1Arrived = false;
        }
        else if(other.CompareTag("Player2"))
        {
            player2Arrived = false;
        }
    }

    private void SetCheckpointForAllPlayers()
    {
        GameObject _player1 = GameObject.FindWithTag("Player1");
        GameObject _player2 = GameObject.FindWithTag("Player2");
        
        SetCheckpoint(_player1, 1);
        SetCheckpoint(_player2, 2);
    }
    
    private void SetCheckpoint(GameObject _player, int _playerId)
    {
        Transform respawnPoint = (_playerId == 1 ? respawnPoint_Player1 : respawnPoint_Player2);
        _player.GetComponent<PlayerDeathRespawn>().SetCheckpoint(respawnPoint.position, respawnPoint.rotation);

        GameObject.FindWithTag(_playerId == 1 ? "SpawnPlayer1" : "SpawnPlayer2").tag = "Untagged";
        respawnPoint.gameObject.tag = (_playerId == 1 ? "SpawnPlayer1" : "SpawnPlayer2");
    }
}
