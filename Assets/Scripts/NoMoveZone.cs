using UnityEngine;

public class NoMoveZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            other.GetComponent<Player>().canMove = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            other.GetComponent<Player>().canMove = true;
        }
    }
}
