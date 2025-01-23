using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class CounterScript : MonoBehaviour
{
    [Header("Counter Settings")]
    public int counter = 0; 

    public WrongScreen wrongScreen;

    [Header("Interactable Objects Settings")]
    public List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable> wrongPaintings; 

    [Header("Action Settings")]
    public GameObject objectToActivate; 

    void Start()
    {
        foreach (var interactable in wrongPaintings)
        {
            if (interactable != null)
            {
                interactable.selectEntered.AddListener(OnObjectSelected);
            }
        }
    }

    private void OnObjectSelected(SelectEnterEventArgs args)
    {
        counter += 1;
        if(counter == 3){
            OnCounterReachedMax();
        }
    }

    private void OnCounterReachedMax()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        foreach (var interactable in wrongPaintings)
        {
            if (interactable != null)
            {
                interactable.selectEntered.RemoveListener(OnObjectSelected);
            }
        }
    }
}
