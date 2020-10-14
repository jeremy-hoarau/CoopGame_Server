using UnityEngine;

public class Bouncy : MonoBehaviour
{
    [SerializeField] private float bounceForce = 600f;
    
    private void OnCollisionEnter(Collision other)
    {
        Vector3 vel = other.rigidbody.velocity;
        
        other.rigidbody.velocity = new Vector3(vel.x, 0, vel.z);
        other.rigidbody.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
    }
}
