using System.Collections;
using UnityEngine;

public class BouncyWall : MonoBehaviour
{
    public float force = 950, yForce = 200;
    
    private bool inCooldown;

    private void OnCollisionEnter(Collision other)
    {
        if(inCooldown)
            return;
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        Vector3 vel = rb.velocity;
        if (rb == null)
            return;
        Vector3 currentDir = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized;
        Vector3 dir = Vector3.Reflect(currentDir, transform.forward).normalized;
        vel.y = 0;
        rb.velocity = Vector3.zero;
        rb.AddForce(dir * force + Vector3.up * yForce, ForceMode.Impulse);
        //rb.AddForce(transform.forward * force + Vector3.up * yForce, ForceMode.Impulse);
        StartCoroutine(Cooldoown());
    }

    IEnumerator Cooldoown()
    {
        inCooldown = true;
        yield return new WaitForSeconds(0.1f);
        inCooldown = false;
    }
}
