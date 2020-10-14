using UnityEngine;

public class RespawnObject : MonoBehaviour
{
    private Vector3 respawnPos;
    private Rigidbody rb;

    private void Start()
    {
        respawnPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    public void Respawn()
    {
        rb.velocity = Vector3.zero;
        transform.position = respawnPos;
    }
}
