using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Town : MonoBehaviour
{

    [SerializeField]
    private GameObject FloatingTextPrefab;


    public void Start()
    {
        this.FloatingTextPrefab.transform.position = new Vector3(this.transform.position.x, 4, this.transform.position.z);

    }

    private void OnMouseOver()
    {

        this.FloatingTextPrefab.SetActive(true);
        this.FloatingTextPrefab.GetComponent<TextMeshPro>().text = "Town";
        // Make the text look at the camera
        Vector3 directionToCamera = FloatingTextPrefab.transform.position - Camera.main.transform.position;

        // Zero out the y component of the direction vector to make sure it doesn't tilt
        directionToCamera.y = 0;

        // Update the rotation of the text to face the camera, only around the Y-axis
        FloatingTextPrefab.transform.rotation = Quaternion.LookRotation(directionToCamera);

    }
    private void OnMouseExit()
    {
        this.FloatingTextPrefab.SetActive(false);
    }
}
