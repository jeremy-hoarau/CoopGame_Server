using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public Animation buttonAnimations;
    public UnityEvent onPress, onRelease;
    
    public void Press()
    {
        buttonAnimations.Play("button_down_animation");
        onPress.Invoke();
    }

    public void Release()
    {
        buttonAnimations.Play("button_up_animation");
        onRelease.Invoke();
    }
}
