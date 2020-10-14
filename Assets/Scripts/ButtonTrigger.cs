using System;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public Button button;
    public int numberOfObjectsNeeded = 1;
    
    private int nbObjects;
    private bool release;

    private void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger)
            return;
        nbObjects++;
        if (nbObjects == numberOfObjectsNeeded)
        {
            button.Press();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.isTrigger)
            return;
        nbObjects--;
        if (nbObjects == numberOfObjectsNeeded - 1)
        {
            button.Release();
            release = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //move a little bit the players that are on the button when it goes up so they can collide with the button and don't stay in it
        if (release)
        {
            if (other.tag.Contains("Player"))
            {
                other.transform.Translate(Vector3.up * 0.001f);
            }
        }
    }

    private void Update()
    {
        if(release)
            release = false;
    }
}
