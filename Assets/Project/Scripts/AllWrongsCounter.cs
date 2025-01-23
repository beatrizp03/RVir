using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using System.Collections.Generic;

public class CounterAndWrongScreen : MonoBehaviour
{
    [Header("Counter Settings")]
    public int counter = 0; 

    [Header("Interactable Objects Settings")]
    public List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable> wrongPaintings; 

    [Header("Action Settings")]
    public GameObject objectMax; // Object to activate when counter reaches max
    public GameObject objectWrong; // Wrong screen object to deactivate
    public float distanceFromUser = 2f; 
    private bool isCoroutineRunning = false; 

    private float objectWrongDisplayDuration = 3f;

    void Start()
    {
        Debug.Log("Counter initialized at: " + counter);

        foreach (var interactable in wrongPaintings)
        {
            if (interactable != null)
            {
                interactable.selectEntered.AddListener(OnWrongPaintingSelected);
            }
        }

    }

    void Update()
    {
        if (objectWrong != null && objectWrong.activeSelf && counter != 3)
        {
            PositionWithUser(objectWrong);
        }else {
            PositionWithUser(objectMax);
        }
    }

    private void OnWrongPaintingSelected(SelectEnterEventArgs args)
    {
        counter++;
        if (counter == 3)
        {
            OnCounterReachedMax();
        }
        else if (objectWrong != null && !isCoroutineRunning)
        {
            objectWrong.SetActive(true);
            StartCoroutine(ShowAndDeactivateTarget());
        }
    }

    private void OnCorrectPaintingSelected(SelectEnterEventArgs args)
    {
        DeactivateobjectWrong();
    }

    private void OnCounterReachedMax()
    {
        if (objectMax != null)
        {
            objectMax.SetActive(true);
        }

        DeactivateobjectWrong();
    }

    private IEnumerator ShowAndDeactivateTarget()
    {
        isCoroutineRunning = true;

        yield return new WaitForSeconds(objectWrongDisplayDuration);

        DeactivateobjectWrong();
        isCoroutineRunning = false;
    }

    private void DeactivateobjectWrong()
    {
        if (objectWrong != null)
        {
            objectWrong.SetActive(false);
        }
    }

    private void PositionWithUser(GameObject targetObject)
    {
        // place screen in front of the user view
        Transform cameraTransform = Camera.main.transform;

        targetObject.transform.position = cameraTransform.position + cameraTransform.forward * distanceFromUser;

        Quaternion lookRotation = Quaternion.LookRotation(targetObject.transform.position - cameraTransform.position);

        Quaternion tiltRotation = Quaternion.Euler(0f, 0f, 0f);

        targetObject.transform.rotation = lookRotation * tiltRotation;
    }
}
