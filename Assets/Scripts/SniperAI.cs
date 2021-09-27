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


    void Start()
    {
        NavMeshHit navHit;
        bool result1 = NavMesh.SamplePosition(transform.position, out navHit, 10f, NavMesh.AllAreas);

        bool result2 = navHit.hit;

        agent.radius += 1;
        agent.radius -= 1;

        if (navHit.hit)
            agent.Warp(navHit.position);

        //Debug.Log(result1 + " " + result2);

        Debug.DrawLine(transform.position, transform.position + Vector3.up * 50, Color.blue, 100f);
        Debug.DrawLine(transform.position, transform.position + Vector3.forward * 50, Color.red, 100f);
    }


    void EscortDestination()
    {
        Vector3 escortLocation = player.transform.position;
        escortLocation += 50f * Random.insideUnitSphere;


        agent.SetDestination(escortLocation);
    }
    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case SNIPER_STATE.ESCORT:
                {

                    //enemy within firing range;
                    if (false)
                    {

                    }
                    // Get within escort distance
                    else if ((agent.destination - player.position).sqrMagnitude > 50f)
                    {
                        EscortDestination();

                    }
                    float sqrDist = (agent.nextPosition - agent.destination).sqrMagnitude;
                    if (sqrDist <= agent.stoppingDistance * agent.stoppingDistance)
                    {
                        EscortDestination();
                    }



                    Debug.DrawLine(agent.destination, agent.destination + Vector3.up * 50, Color.blue, 100f);
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


        Vector3 input = transform.InverseTransformDirection(agent.velocity.normalized);

        animator.SetBool("isWalking", Mathf.Abs(input.x) > 0 || Mathf.Abs(input.z) > 0);
        animator.SetFloat("horizontal", input.x);
        animator.SetFloat("vertical", input.z);

        Debug.DrawLine(transform.position, transform.position + input * 50, Color.red);

    }
}
