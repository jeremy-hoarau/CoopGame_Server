using UnityEngine;

public class Doors : MonoBehaviour
{
    enum DoorType
    {
        doors,
        simpleDoor
    }

    [SerializeField] private DoorType doorType = DoorType.doors;
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
            if (doorType == DoorType.doors)
            {
                doorsAnimations.Play("doors_open");
            }
            else if (doorType == DoorType.simpleDoor)
            {
                doorsAnimations.Play("simple_door_open");
            }
            isOpen = true;
        }
    }

    public void Close()
    {
        nbOfActivators--;

        if (!isOpen) return;

        if (nbOfActivators == nbOfActivatorsNeeded - 1)
        {
            if (doorType == DoorType.doors)
            {
                doorsAnimations.Play("doors_close");
            }
            else if (doorType == DoorType.simpleDoor)
            {
                doorsAnimations.Play("simple_door_close");
            }
            isOpen = false;
        }
    }
}
