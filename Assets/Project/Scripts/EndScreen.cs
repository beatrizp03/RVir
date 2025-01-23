using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; 
using System.Collections.Generic;

public class EndScreen : MonoBehaviour
{
    public GameObject targetObject; 

    public GameObject map;

    public float distanceFromUser = 2f; 
    public float verticalOffset = 0.2f;

    // List of interactable objects to disable
    public List<GameObject> interactableObjects = new List<GameObject>();

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable simpleInteractable; 

    public RandomizeMaps randomizeMaps;

    public GameObject[] endText;

    public GameObject continueButton;
    
    void Update()
    {
        if(randomizeMaps != null && !endText[0].activeSelf && !endText[1].activeSelf){
            if (randomizeMaps.currentIndex == 2){
                endText[1].SetActive(true); //last mode being tested
            } else {
                endText[0].SetActive(true);
                continueButton.SetActive(true);
            }
        }
        if (targetObject != null)
        {
            PositionWithUser();
        }

        if (map != null && map.activeSelf)
        {
            map.SetActive(false); // Deactivate the map
        }

        // Loop through the list and disable interaction for each object
        foreach (var interactableObject in interactableObjects)
        {
            simpleInteractable = interactableObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();

            if (simpleInteractable != null && simpleInteractable.enabled)
            {
                simpleInteractable.enabled = false;
            }
        }
    }

    private void PositionWithUser()
    {
        Transform cameraTransform = Camera.main.transform;

        Vector3 offsetPosition = cameraTransform.position + cameraTransform.forward * distanceFromUser;
        offsetPosition.y += verticalOffset; 

        targetObject.transform.position = offsetPosition;

        Quaternion lookRotation = Quaternion.LookRotation(targetObject.transform.position - cameraTransform.position);

        targetObject.transform.rotation = lookRotation;
    }
}
