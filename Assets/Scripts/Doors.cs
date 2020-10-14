using UnityEngine;

public class Doors : MonoBehaviour
{
    public Animation doorsAnimations;
    public int nbOfActivatorsNeeded = 1;
    
    private int nbOfActivators;
    private bool isOpen;

    public void Open()
    {
        nbOfActivators++;

        if (isOpen) return;
        
        if(nbOfActivators == nbOfActivatorsNeeded)
        {
            doorsAnimations.Play("doors_open");
            isOpen = true;
        }
    }

    public void Close()
    {
        nbOfActivators--;

        if (!isOpen) return;

        if (nbOfActivators == nbOfActivatorsNeeded - 1)
        {
            doorsAnimations.Play("doors_close");
            isOpen = false;
        }
    }
}
