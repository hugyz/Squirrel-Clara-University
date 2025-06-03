using UnityEngine;

public class RobloxStyleCamera : MonoBehaviour
{
    public Transform target;
    public Transform cameraTransform;

    public float distance = 40f;
    public float minDistance = 3f;
    public float maxDistance = 40f;
    public float height = 2f;
    public float sensitivity = 2f;
    public float pitchClamp = 89f;
    public float smoothSpeed = 10f;

    private float yaw = 0f;
    private float pitch = 20f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        distance = maxDistance;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, pitchClamp);

        if (pitch < 1f)
        {
            float t = Mathf.InverseLerp(1f, -90f, pitch);
            distance = Mathf.Lerp(maxDistance, minDistance, t);
        }
        else if (distance != maxDistance)
        {
            distance = maxDistance;
        }
    }

    void LateUpdate()
    {
        Vector3 targetPosition = target.position + Vector3.up * height;
        Vector3 finalPosition;

        if (pitch >= 1f)
        {
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 orbitDirection = rotation * Vector3.back;
            finalPosition = targetPosition + orbitDirection * distance;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, finalPosition, Time.deltaTime * smoothSpeed);
            cameraTransform.LookAt(targetPosition);
        }
        else
        {
            float t = Mathf.InverseLerp(1f, -90f, pitch);
            Vector3 baseDirection = Quaternion.Euler(1f, yaw, 0f) * Vector3.back;
            Vector3 orbitPos = targetPosition + baseDirection * distance;
            Vector3 closeUpPos = targetPosition + Vector3.up * 0.5f;
            finalPosition = Vector3.Lerp(orbitPos, closeUpPos, t);
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, finalPosition, Time.deltaTime * smoothSpeed);
            float upwardPitch = Mathf.Lerp(1f, 89f, t);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, Quaternion.Euler(-upwardPitch, yaw, 0f), Time.deltaTime * (smoothSpeed * 0.5f));
        }
    }
}
