using UnityEngine;

public class ModifyBoxMassInTrigger : MonoBehaviour
{
    public float newMass;
    
    private float defaultMass;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            defaultMass = other.gameObject.GetComponent<Rigidbody>().mass;
            other.gameObject.GetComponent<Rigidbody>().mass = newMass;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            other.gameObject.GetComponent<Rigidbody>().mass = defaultMass;
        }
    }
}
