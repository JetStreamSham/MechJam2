using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCam : MonoBehaviour
{
    public Transform lookAt;
    public Transform camTransform;

    private Camera cam;

    public  float distance = 4.0f;
    private float currentX = 0.0f;
    public float currentY = 8.0f;
    public float sensivityX = 5.0f;
    public float sensivityY = 1.0f;

    private void Start()
    {

        camTransform = transform;
        cam = Camera.main;

    }

    private void Update()
    {
        currentX += Input.GetAxis("Mouse X");
        currentY += Input.GetAxis("Mouse Y");
    }


    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX * sensivityX, 0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }
}
