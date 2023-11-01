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
        this.FloatingTextPrefab.transform.LookAt(FloatingTextPrefab.transform.position - Camera.main.transform.position);


    }
    private void OnMouseExit()
    {
        this.FloatingTextPrefab.SetActive(false);
    }
}
