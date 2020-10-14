using UnityEngine;

public class MovePlayerToSpawn : MonoBehaviour
{
    [SerializeField] [Range(1, 2)] private int playerId = 1;
    
    private void Start()
    {
        if(Server.clients.ContainsKey(playerId) && Server.clients[playerId].tcp.socket != null)
        {
            PlayerDeathRespawn _playerRespawn = Server.clients[playerId].player.GetComponent<PlayerDeathRespawn>();
            _playerRespawn.SetCheckpoint(transform.position, transform.rotation);
            _playerRespawn.Respawn();
        }
    }
}
