using UnityEngine;

public class PlayerDeathRespawn : MonoBehaviour
{
    public Vector3 respawnPoint;
    public Quaternion respawnDirection;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        respawnPoint = transform.position;
        respawnDirection = transform.rotation;
    }

    public void Die()
    {
        Respawn();
    }

    public void Respawn()
    {
        rb.velocity = Vector3.zero;
        transform.position = respawnPoint;
        ServerSend.RotatePlayer(GetComponent<Player>().id, respawnDirection);
    }

    public void SetCheckpoint(Vector3 _checkpoint, Quaternion _direction)
    {
        respawnPoint = _checkpoint;
        respawnDirection = _direction;
    }
}
