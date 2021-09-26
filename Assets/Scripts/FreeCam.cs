using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCam : MonoBehaviour
{
    public Camera cam;
    public CameraSettings camSettings;
    public float defaultSpeed;
    public float currentSpeed;

    public Vector3 inputVector;
    public Vector3 motionVector;

    public Vector2 mouseVector;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        currentSpeed = defaultSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        Rotate();
        Move();
    }


    void Rotate()
    {
        mouseVector.x += Input.GetAxis("Mouse X") * camSettings.sensitivty;
        mouseVector.y += Input.GetAxis("Mouse Y") * camSettings.sensitivty;

        mouseVector.y = Mathf.Clamp(mouseVector.y, -camSettings.yMaxAngle, camSettings.yMaxAngle);

        var xQuat = Quaternion.AngleAxis(mouseVector.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(mouseVector.y, Vector3.left);

        transform.localRotation = xQuat * yQuat; 


    }

    void Move()
    {
        float speedDelta = Input.mouseScrollDelta.y;
        currentSpeed += speedDelta;
        Mathf.Clamp(currentSpeed, 0, float.PositiveInfinity);

        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.z = Input.GetAxis("Vertical");


        motionVector = transform.rotation * inputVector;

        motionVector *= currentSpeed;
        //Debug.Log($"Motion Vector{motionVector}");
        transform.position += motionVector;

    }
}
