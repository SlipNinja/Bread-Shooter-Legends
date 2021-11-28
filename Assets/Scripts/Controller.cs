using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private float dragSpeed = 150f;
    private Vector3 dragOrigin;
 
    void Start()
    {

    }
 
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X") * dragSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * dragSpeed * Time.deltaTime;

            Vector3 newPos = new Vector3(mouseX, mouseY, 0);

            transform.Translate(-newPos);
        }
    }
}
