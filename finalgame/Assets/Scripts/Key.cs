using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Key : MonoBehaviour
{
    public float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.localScale = new Vector3(0,0,0);
    }

    // Update is called once per frame
    public void SetActive()
    {
        this.gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, rotateSpeed, 0);
    }

}
