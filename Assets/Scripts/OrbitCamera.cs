using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;  // Drag your player's transform here in the inspector
    public float rotationSpeed = 50.0f;  // Adjust to set the desired rotation speed

    private void Update()
    {
        if (target == null) return;

        // Calculate the relative position of the camera to the player
        Vector3 relativePos = target.position - transform.position;

        if (Input.GetKey(KeyCode.A))
        {
            // Rotate around the player to the left
            transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            // Rotate around the player to the right
            transform.RotateAround(target.position, Vector3.up, -rotationSpeed * Time.deltaTime);
        }

        // Maintain focus on the player
        transform.LookAt(target.position);
    }
}
