using UnityEngine;

public class RoomTeleport : MonoBehaviour
{
    [Header("Target Room Position")]
    public Vector3 targetRoomPosition; 

    public Transform cameraTransform; 

    public void TeleportToRoom()
    {
        if (cameraTransform != null)
        {
            cameraTransform.position = targetRoomPosition;

        }
    }
}
