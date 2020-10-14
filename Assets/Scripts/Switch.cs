using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{    
    public Animation switchAnimations;
    public UnityEvent onSwitchDown, onSwitchUp;
    
    public void SwitchToDown()
    {
        onSwitchDown.Invoke();
        switchAnimations.Play("switch_down");
    }
    
    public void SwitchToUp()
    {
        onSwitchUp.Invoke();
        switchAnimations.Play("switch_up");
    }

    public void VisualToDown()
    {
        switchAnimations.Play("switch_down");
    }
    
    public void VisualToUp()
    {
        switchAnimations.Play("switch_up");
    }
}
