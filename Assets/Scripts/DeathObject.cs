using UnityEngine;

public class DeathObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            other.gameObject.GetComponent<PlayerDeathRespawn>().Die();
        }

        if (other.gameObject.CompareTag("Box"))
        {
            other.gameObject.GetComponent<RespawnObject>().Respawn();
        }
    }
}
