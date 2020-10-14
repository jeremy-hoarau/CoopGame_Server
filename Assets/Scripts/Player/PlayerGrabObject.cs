using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGrabObject : MonoBehaviour
{
    public Vector3 objectPositionOffsetWhenGrabbed = new Vector3(0, 1f, 1.5f);
    public bool isGrabbing;
    public float speedToRecenter = 300f,
        maxDistance = 6f;
    
    private Player player;
    private bool isTryingToGrab;
    private List<GameObject> inRangeObjects = new List<GameObject>();
    private GameObject grabbedObject;
    private Rigidbody grabbedObjectRb;
    private Bounds bounds;
    private float lastY;
    private int id;

    private void Start()
    {
        player = GetComponent<Player>();
        id = player.id;
    }

    private void Update()
    {
        if (!isGrabbing && inRangeObjects.Count > 0 && player.isTryingToGrab)
        {
            StartGrab();
        }
        else if (isGrabbing && !player.isTryingToGrab)
        {
            StopGrab();
        }
    }

    private void FixedUpdate()
    {
        if (isGrabbing && (Vector3.Distance(grabbedObject.transform.position, transform.position) > maxDistance || grabbedObject == null))
        {
            StopGrab();
        }
        if (isGrabbing)
        {
            if (lastY != transform.position.y)
            {
                //adapt the object's height to the player's height manually because it has a constraint on y when grabbed
                grabbedObject.transform.position = new Vector3(grabbedObject.transform.position.x,
                    transform.position.y + objectPositionOffsetWhenGrabbed.y + bounds.size.y / 2, grabbedObject.transform.position.z);
                lastY = transform.position.y;
            }
            grabbedObjectRb.velocity = (transform.position + transform.TransformVector(
                objectPositionOffsetWhenGrabbed + new Vector3(0, bounds.size.y / 2,
                    bounds.size.z / 2)) - grabbedObject.transform.position) * (speedToRecenter * Time.fixedDeltaTime);
        }
        
        ServerSend.PlayerGrabbingState(id, isGrabbing);
    }

    private void StartGrab()
    {
        float lastDistance = 999;
        Vector3 pos = transform.position;

        grabbedObject = null;
        foreach (GameObject go in inRangeObjects)
        {
            if (go != null && Vector3.Distance(pos, go.transform.position) < lastDistance && !go.GetComponent<GrabbableObject>().isGrabbed)
            {
                lastDistance = Vector3.Distance(pos, go.transform.position);
                grabbedObject = go;
            }
        }
        if(grabbedObject == null)
            return;
        
        isGrabbing = true;
        GrabbableObject grabbableObject = grabbedObject.GetComponent<GrabbableObject>();
        grabbableObject.isGrabbed = true;
        grabbableObject.playerGrab = this;
        grabbedObjectRb = grabbedObject.GetComponent<Rigidbody>();
        bounds = grabbedObjectRb.GetComponent<Collider>().bounds;
        //grabbedObject.transform.position = pos + transform.TransformVector(objectPositionOffsetWhenGrabbed + new Vector3(0, bounds.size.y / 2, bounds.size.z / 2));
        grabbedObject.transform.position = new Vector3(grabbedObject.transform.position.x,
            transform.position.y + objectPositionOffsetWhenGrabbed.y + bounds.size.y / 2, grabbedObject.transform.position.z);
        grabbedObjectRb.constraints = RigidbodyConstraints.FreezePositionY;
    }

    private void StopGrab()
    {
        GrabbableObject grabbableObject = grabbedObject.GetComponent<GrabbableObject>();
        grabbableObject.isGrabbed = false;
        grabbableObject.playerGrab = null;
        grabbedObjectRb.constraints = RigidbodyConstraints.None;
        grabbedObject = null;
        grabbedObjectRb = null;
        isGrabbing = false;
    }

    public void DestroyGrabbedObject()
    {
        if(!isGrabbing)
            return;
        inRangeObjects.Remove(grabbedObject);
        Destroy(grabbedObject);
        grabbedObject = null;
        grabbedObjectRb = null;
        isGrabbing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            inRangeObjects.Add(other.gameObject);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            inRangeObjects.Remove(other.gameObject);
        }
    }
}
