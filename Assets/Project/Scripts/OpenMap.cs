using UnityEngine;
using UnityEngine.XR;

public class ToggleMap : MonoBehaviour
{
    public GameObject targetObject; 
    private bool canToggle = true; 
    private float toggleCooldown = 0.5f; 

    public float distanceFromUser = 3f; 
    public float mapScaleFactor = 0.025f; 

    public GameObject userDot; 
    public Transform mapTransform; 

    private Vector3 previousUserPosition; 

    public Vector3 mapRotation;

    void Start()
    {
        previousUserPosition = Camera.main.transform.position;
    }

    void Update()
    {
        if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand)
                        .TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed) && isPressed)
        {
            if (canToggle)
            {
                ToggleObject();
                StartCooldown();
            }
        }

        if (targetObject != null && targetObject.activeSelf)
        {
            PositionWithUser();
            UpdateUserDotPosition(); 
        }
    }

    private void ToggleObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf); 
            userDot.SetActive(!userDot.activeSelf);
        }
        else
        {
            Debug.LogWarning("Target object is not assigned!");
        }
    }

    private void StartCooldown()
    {
        canToggle = false; 
        Invoke(nameof(ResetCooldown), toggleCooldown); 
    }

    private void ResetCooldown()
    {
        canToggle = true; 
    }

    private void PositionWithUser()
    {
        Transform cameraTransform = Camera.main.transform;

        targetObject.transform.position = cameraTransform.position + cameraTransform.forward * distanceFromUser;

        Quaternion lookRotation = Quaternion.LookRotation(targetObject.transform.position - cameraTransform.position);

        Quaternion tiltRotation = Quaternion.Euler(mapRotation);

        targetObject.transform.rotation = lookRotation * tiltRotation;
    }

    private void UpdateUserDotPosition()
    {
        if (userDot == null || mapTransform == null)
        {
            return;
        }

        Vector3 currentUserPosition = Camera.main.transform.position;

        Vector3 movementDelta = currentUserPosition - previousUserPosition;

        Vector3 adjustedMovementDelta = mapTransform.TransformDirection(movementDelta);

        Vector3 mapMovementDelta = adjustedMovementDelta * mapScaleFactor;

        userDot.transform.position += mapMovementDelta;

        previousUserPosition = currentUserPosition;
    }

}
