using UnityEngine;

public class MouseScrollZoom : MonoBehaviour
{
    public Transform cameraTransform;
    public float zoomSpeed = 5f;
    public float minDistance = 2f;
    public float maxDistance = 10f;
    public float smoothSpeed = 5f; 

    private Vector3 offsetDirection;
    private float targetDistance;
    private float currentDistance;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = transform;
        }
        offsetDirection = cameraTransform.localPosition.normalized;
        currentDistance = cameraTransform.localPosition.magnitude;
        targetDistance = currentDistance;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            targetDistance -= scroll * zoomSpeed;
            targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        }

        //Smoothly interpolate toward targetDistance
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * smoothSpeed);
        cameraTransform.localPosition = offsetDirection * currentDistance;
    }
}
