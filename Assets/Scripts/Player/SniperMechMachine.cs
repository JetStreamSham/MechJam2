using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperMechMachine : MonoBehaviour
{
    public Camera camera;
    public Rigidbody rigidbody;
    public Animator animator;

    public float walkSpeed = 1f;
    public float angularSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (Input.GetButtonUp("Fire1"))
            animator.SetTrigger("shoot");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("isWalking", Mathf.Abs(input.z) > 0.001f || Mathf.Abs(input.x) > 0.001f);
            animator.SetFloat("horizontal", input.x);
            animator.SetFloat("vertical", input.z);
            input = transform.rotation * input;
            transform.Translate(input * walkSpeed * Time.deltaTime, Space.World);

        }
        else
        {
            animator.SetBool("isWalking", Mathf.Abs(input.z) > 0.001f);
            animator.SetBool("isRotating", Mathf.Abs(input.x) > 0.001f);
            animator.SetFloat("horizontal", input.x);
            animator.SetFloat("vertical", input.z);
            //input = transform.rotation * input;


            transform.Rotate(input.x * transform.up, angularSpeed * Time.deltaTime);
            Vector3 velocity = (transform.forward * input.z * walkSpeed * Time.deltaTime);

            Debug.Log($"{velocity}");

            transform.Translate(transform.forward * input.z * walkSpeed * Time.deltaTime, Space.World);
        }
    }
}
