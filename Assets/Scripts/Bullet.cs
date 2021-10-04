using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public float knockback = 1.25f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 200);
    }

    // Update is called once per frame
    void Update()
    {
       // transform.position += transform.forward;
    }

    public void ApplyKnockBack(Rigidbody rb)
    {

    }
}
