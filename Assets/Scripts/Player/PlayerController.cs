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

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (health >= 0)
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
        } else
        {
            Explode();
        }


    }

    void Explode()
    {
        GameObject a = GameObject.Instantiate(explode, transform.position + Vector3.up * 10, transform.rotation);
        a.transform.localScale *= 10f;
        // Debug.Break();
        Destroy(a, 5f);
        Destroy(gameObject);

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

        instancedBullet.GetComponent<Bullet>().damage = 1;
        bulletBody.AddForce(ray.direction * 50000f);
        Destroy(instancedBullet, 25);
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Caller:{gameObject.name} Collider: {other.name}");
        if (other.tag == "enemy")
        {
            health -= 30;
        }
    }
}
