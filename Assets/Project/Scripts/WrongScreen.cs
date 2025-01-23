using UnityEngine;
using System.Collections;

public class WrongScreen : MonoBehaviour
{
    public GameObject targetObject; 
    public float distanceFromUser = 2f;
    private bool isCoroutineRunning = false; 
    public CounterScript counterScript;

    private void Start(){
        if (counterScript.counter == 3){
            targetObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (targetObject != null && targetObject.activeSelf && !isCoroutineRunning)
        {
            StartCoroutine(ShowAndDeactivateTarget());
            PositionWithUser();
        }
    }

    private IEnumerator ShowAndDeactivateTarget()
    {
        isCoroutineRunning = true;

        yield return new WaitForSeconds(3f);

        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }

        isCoroutineRunning = false;
    }

    private void PositionWithUser()
    {
        Transform cameraTransform = Camera.main.transform;

        targetObject.transform.position = cameraTransform.position + cameraTransform.forward * distanceFromUser;

        Quaternion lookRotation = Quaternion.LookRotation(targetObject.transform.position - cameraTransform.position);

        Quaternion tiltRotation = Quaternion.Euler(0f, 0f, 0f);

        targetObject.transform.rotation = lookRotation * tiltRotation;
    }
}
