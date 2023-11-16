using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;  // Drag your player's transform here in the inspector
    public float rotationSpeed = 50.0f;  // Adjust to set the desired rotation speed
    public bool zoom = false;
    public Vector3 oldPos;
    [SerializeField]
    public unit player;

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
        if (zoom)
        {
            
            zoomTo(target.position, oldPos);
        }


        // Maintain focus on the player
        transform.LookAt(target.position);
    }

    public void zoomTo(Vector3 pos, Vector3 oldPosition)
    {
        Vector3 newTarget = new Vector3(pos.x, pos.y +7, pos.z);
        transform.position = Vector3.MoveTowards(transform.position, newTarget, 3 * Time.deltaTime);
        if (transform.position == newTarget)
        {
            zoom = false;
            target = player.transform;
            transform.position = oldPosition;
        }

    }

}
