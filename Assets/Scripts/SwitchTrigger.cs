using System.Collections;
using UnityEngine;


public class SwitchTrigger : MonoBehaviour
{
    public enum State
    {
        up,
        down
    }

    public float triggerCD = 0.2f;
    public Switch switch_button;

    public State state;
    private bool canBeTriggered = true;
    
    private void OnTriggerStay(Collider other)
    {
        if (canBeTriggered && other.tag.Contains("Player"))
        {
            if (other.GetComponent<Player>().isTryingToInteract)
            {
                state = (state == State.down ? State.up : State.down);
                if(state == State.down)
                    switch_button.SwitchToDown();
                else
                    switch_button.SwitchToUp();
                
                StartCoroutine(TriggerCD());
            }
        }
    }

    IEnumerator TriggerCD()
    {
        canBeTriggered = false;
        yield return new WaitForSeconds(triggerCD);
        canBeTriggered = true;
    }

    public void ChangeToUpState()
    {
        if (state == State.down)
        {
            switch_button.VisualToUp();
            state = State.up;

            StartCoroutine(TriggerCD());
        }
    }
    
    public void ChangeToDownState()
    {
        if (state == State.up)
        {
            switch_button.VisualToDown();
            state = State.down;
            
            StartCoroutine(TriggerCD());
        }
    }
}
