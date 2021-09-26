using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public enum SNIPER_STATE
{
    /// <summary>
    /// Loosley follow behind player and shoot at things
    /// </summary>
    ESCORT,
    /// <summary>
    /// hide behind obstacles,shoot enemies from far away 
    /// </summary>
    COMBAT,
    /// <summary>
    /// keep hiding, slow enemies down,cover fire until everyone got in barrier
    /// </summary>
    RETREAT,
};


[RequireComponent(typeof(NavMeshAgent))]
public class SniperAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Rigidbody rigidbody;
    public Animator animator;

    public Transform player;
    public Transform target;

    public SNIPER_STATE state;

    public float walkSpeed = 1f;
    public float angularSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case SNIPER_STATE.ESCORT:
                {
                    if(target != player)
                    {
                        target = player;
                        Vector3 escortLocation = target.transform.position;
                        escortLocation += 50f * Random.insideUnitSphere;
                        agent.SetDestination(escortLocation);
                    }
                    break;
                }
            case SNIPER_STATE.COMBAT:
                {
                    break;
                }
            case SNIPER_STATE.RETREAT:
                {
                    break;
                }
        }


        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        animator.SetBool("isWalking", Mathf.Abs(input.z) > 0.001f);
        animator.SetBool("isRotating", Mathf.Abs(input.x) > 0.001f);
        animator.SetFloat("horizontal", input.x);
        animator.SetFloat("vertical", input.z);



    }
}
