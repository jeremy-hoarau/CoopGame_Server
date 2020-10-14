using UnityEngine;
using UnityEngine.Events;

public class Elevator : MonoBehaviour
{
    public Vector3 endOffset;
    public float speed = 4f;
    public float arrivalDetectionDistance = 0.2f;

    public UnityEvent onMoveToEndPosition, onMoveToInitPosition;
    
    private Vector3 initPos;
    private Vector3 endPos;
    
    private bool moveToEndPos, moveToInitPos;
    
    void Awake()
    {
        initPos = transform.position;
        endPos = initPos + endOffset;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;    //also needed for the player because it has to be at the root to be added to DontDestroyOnLoad
        if (other.tag.Contains("Player"))
            DontDestroyOnLoad(other);
    }

    void FixedUpdate()
    {
        if (moveToEndPos)
        {
            Vector3 pos = transform.position;
            float distanceToEndPos = Vector3.Distance(pos, endPos);
            
            if (distanceToEndPos < arrivalDetectionDistance)
            {
                transform.position = endPos;
                moveToEndPos = false;
            }
            else
            {
                transform.Translate((endPos-pos).normalized * (speed * Time.fixedDeltaTime));
            }
        }
        else if (moveToInitPos)
        {
            Vector3 pos = transform.position;
            float distanceToInitPos = Vector3.Distance(pos, initPos);

            if (distanceToInitPos < arrivalDetectionDistance)
            {
                transform.position = initPos;
                moveToInitPos = false;
            }
            else
            {
                transform.Translate((initPos-pos).normalized * (speed * Time.fixedDeltaTime));
            }
        }
    }

    public void moveToEndPosition()
    {
        moveToInitPos = false;
        moveToEndPos = true;
        onMoveToEndPosition.Invoke();
    }

    public void moveToInitPosition()
    {
        moveToEndPos = false;
        moveToInitPos = true;
        onMoveToInitPosition.Invoke();
    }
}
