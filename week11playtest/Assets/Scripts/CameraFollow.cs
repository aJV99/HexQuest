using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    private Vector3 cameraOffset;
    public float smoothFactor = 0.5f;
    public float rotationSpeed = 5.0f;

    private bool isPanning = false;
    private bool inMapView = false;

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    private Vector3 mapViewPosition = new Vector3(16.089f, 41f, -4.328323f);
    private Quaternion mapViewRotation = Quaternion.Euler(89.98f, 0f, 0f);

    public GameObject UIBar;
    public GameObject Notif;
    public GameObject RaycastBlocker;


    void Start()
    {
        cameraOffset = transform.position - playerTransform.position;
        originalCameraPosition = transform.position;
        originalCameraRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!inMapView)
            {
                // Save the current position before moving to map view
                originalCameraPosition = transform.position;
                originalCameraRotation = transform.rotation;
                StartCoroutine(MoveToPosition(mapViewPosition, mapViewRotation, 1.0f));
                inMapView = true;
                UIBar.gameObject.SetActive(false);
                Notif.gameObject.SetActive(false);
                RaycastBlocker.gameObject.SetActive(true);
            }
            else
            {
                StartCoroutine(MoveToPosition(originalCameraPosition, originalCameraRotation, 1.0f));
                inMapView = false;
                UIBar.gameObject.SetActive(true);
                Notif.gameObject.SetActive(true);
                RaycastBlocker.gameObject.SetActive(false);
            }
        }

        if (!isPanning && !inMapView)
        {
            // Orbit functionality
            if (Input.GetKey(KeyCode.D))
            {
                cameraOffset = Quaternion.AngleAxis(-rotationSpeed * Time.deltaTime, Vector3.up) * cameraOffset;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                cameraOffset = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.up) * cameraOffset;
            }

            Vector3 newPosition = playerTransform.position + cameraOffset;
            transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);
            transform.LookAt(playerTransform);
        }
    }

    public IEnumerator PanCameraToObject(GameObject targetObject)
    {
        Debug.Log("Coroutine started: Panning to object.");
        isPanning = true;

        // Save the original camera position and rotation
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;

        // Calculate the target position and rotation relative to the target object
        Vector3 targetPositionOffset = new Vector3(-7.211359f, 11.66f, -6.627083f);
        Quaternion targetRotationOffset = Quaternion.Euler(49.971f, 47.418f, 0f);
        Vector3 targetPosition = targetObject.transform.TransformPoint(targetPositionOffset);
        Quaternion targetRotation = targetObject.transform.rotation * targetRotationOffset;

        float timeToFocus = 1.0f; // Duration of the focus animation

        // Animate camera to the target
        float t = 0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime / timeToFocus;
            transform.position = Vector3.Lerp(originalPosition, targetPosition, t);
            transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, t);
            yield return null;
        }

        Debug.Log("Focus on object achieved. Starting focus timer.");

        // Keep the focus on the target object for 2 seconds
        float focusTimeElapsed = 0f;
        while (focusTimeElapsed < 2.0f)
        {
            focusTimeElapsed += Time.deltaTime;
            transform.position = targetPosition;
            transform.rotation = targetRotation;
            yield return null;
        }

        Debug.Log("Focus duration ended. Returning to player.");

        // Return the camera to the player
        t = 0f; // Reset t to 0 for the return journey
        while (t < 1.0f)
        {
            t += Time.deltaTime / timeToFocus;
            transform.position = Vector3.Lerp(targetPosition, originalPosition, t);
            transform.rotation = Quaternion.Slerp(targetRotation, originalRotation, t);
            yield return null;
        }

        Debug.Log("Coroutine finished: Camera returned to player.");
        isPanning = false;
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        isPanning = true;
        float startTime = Time.time;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        isPanning = false;
    }
}
