using UnityEngine;

public class SwapPlatform : MonoBehaviour
{
    public int id = 1;
    public bool activated;
    public GameObject defaultPlatform, activatedPlatform;

    private int nbOfActivators;

    private void Start()
    {
        activated = false;
        defaultPlatform.SetActive(true);
        activatedPlatform.SetActive(false);
    }

    public void Activate()
    {
        nbOfActivators++;
        
        defaultPlatform.SetActive(false);
        activatedPlatform.SetActive(true);
        activated = true;
        ServerSend.SwapPlatformState(id, activated);
    }

    public void Deactivate()
    {
        nbOfActivators--;
        if(nbOfActivators > 0)
            return;
        
        defaultPlatform.SetActive(true);
        activatedPlatform.SetActive(false);
        activated = false;
        ServerSend.SwapPlatformState(id, activated);
    }
}
