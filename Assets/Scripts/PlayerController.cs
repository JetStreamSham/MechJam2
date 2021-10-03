using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public Rigidbody rigidbody;
    public Animator animator;
    public Camera camera;

    public Transform barrel;
    public GameObject bullet;

    public float health = 100;

    public float shootTime;
    public float currentShootTime;

    public float walkSpeed = 1f;

    public float shootCooldown;
    public float currentShootCooldown;

    public GameObject explode;
    public CameraSettings camSettings;
    Vector2 mouseInput;

    bool shooting;


    public Texture2D cursor;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 dims = new Vector2(cursor.width,cursor.height);
        dims /= .5f;

        Cursor.SetCursor(cursor,dims,CursorMode.Auto);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = Vector3.zero;
        input.x = Input.GetAxis("Horizontal");
        input.z = Input.GetAxis("Vertical");

        animator.SetFloat("walkMultiplier", characterController.velocity.magnitude / 10);
        animator.SetFloat("horizontal", input.x);
        animator.SetFloat("vertical", input.z);
        Rotate();

        input = transform.rotation * input;
        Vector3 movement = input * walkSpeed * Time.deltaTime;

        movement.y = -9.8f * Time.deltaTime;

        characterController.Move(movement);

        if (Input.GetButtonDown("Fire1"))
            shooting = true;
        if (shooting && Input.GetButton("Fire1"))
        {
            if (currentShootTime >= 0)
            {
                Shoot();
                currentShootTime -= Time.deltaTime;
            }
        }

        if (currentShootTime <= 0)
        {
            shooting = false;
            if (currentShootCooldown >= 0)
            {
                currentShootCooldown -= Time.deltaTime;
            }
            else
            {
                currentShootCooldown = shootCooldown;
                currentShootTime = shootTime;
            }

        }


    }

    void Shoot()
    {
        //shoot em
        animator.Play("shooting upper", 1);
        GameObject instancedBullet = GameObject.Instantiate(bullet);
        instancedBullet.name = gameObject.name + " Bullet";
        instancedBullet.tag = "player";
        instancedBullet.transform.position = barrel.position + barrel.forward;
        float x = Screen.width / 2;
        float y = Screen.height / 2;

        var ray = camera.ScreenPointToRay(new Vector3(x, y, 0));
        Rigidbody bulletBody = instancedBullet.GetComponent<Rigidbody>();

        bulletBody.AddForce(ray.direction * 50000f);
        Destroy(instancedBullet, 100);
    }

    void Rotate()
    {
        mouseInput.x += Input.GetAxis("Mouse X") * camSettings.sensitivty;
        //mouseInput.y += Input.GetAxis("Mouse Y") * camSettings.sensitivty;

        mouseInput.y = Mathf.Clamp(mouseInput.y, -camSettings.yMaxAngle, camSettings.yMaxAngle);

        var xQuat = Quaternion.AngleAxis(mouseInput.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(mouseInput.y, Vector3.left);

        transform.rotation = xQuat;


    }
}
