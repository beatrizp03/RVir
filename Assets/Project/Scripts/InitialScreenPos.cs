using UnityEngine;

public class InitialScreenPos : MonoBehaviour
{
    public GameObject targetObject; 
    public float distanceFromUser = 2f; 
    public float verticalOffset = 0.2f; 

    private void Update()
    {
        if (targetObject != null && targetObject.activeSelf)
        {
            PositionWithUser();
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
