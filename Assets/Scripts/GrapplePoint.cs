using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    [SerializeField] [Range(1, 2)] private int playerId = 1;
    private bool isInRange, isGrappling;
    private GameObject player_go;
    private Player player;
    private SpringJoint joint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerId == 1 ? "Player1" : "Player2"))
        {
            isInRange = true;
            
            if (player_go != null) return;
            player_go = GameObject.FindWithTag(playerId == 1 ? "Player1" : "Player2");
            player = player_go.GetComponent<Player>();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerId == 1 ? "Player1" : "Player2"))
        {
            isInRange = false;
        }
    }

    private void Update()
    {
        if (isInRange && !isGrappling && player.isTryingToGrapple && !(player_go.transform.position.y > transform.position.y))
        {
            StartGrappling();
        }
        else if(isGrappling && (!player.isTryingToGrapple || player_go.transform.position.y > transform.position.y))
        {
            StopGrappling();
        }
    }

    private void StartGrappling()
    {
        isGrappling = true;
        Vector3 pos = transform.position;
        ServerSend.PlayerStartGrappling(player.id, pos);

        joint = player_go.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = pos;

        float distanceToPlayer = Vector3.Distance(player_go.transform.position, pos);
        joint.maxDistance = distanceToPlayer * 0.8f;
        joint.minDistance = distanceToPlayer * 0.25f;

        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 135f;
    }

    private void StopGrappling()
    {
        isGrappling = false;
        ServerSend.PlayerStopGrappling(player.id);

        Destroy(joint);
    }
}
