using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum HunterState
{
    PATROL,
    ATTACK,
}

public class HunterAI : Enemy
{
    public Animator animator;
    public NavMeshAgent agent;
    public HunterState state;


    public SniperAI aiTarget;
    public Transform target;

    public float agroRange;
    public float attackRange;

    public Collider clbox;
    public GameObject attackBox;

    public float attackCooldown;
    public float curremtAttackCooldown;

    public Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        NavMeshHit navHit;
        bool result1 = NavMesh.SamplePosition(transform.position, out navHit, 10f, NavMesh.AllAreas);

        bool result2 = navHit.hit;

        agent.radius += 1;
        agent.radius -= 1;

        if (navHit.hit)
            agent.Warp(navHit.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth >= 0)
        {
            if (curremtAttackCooldown > 0)
            {
                curremtAttackCooldown -= Time.deltaTime;
            }

            switch (state)
            {
                case HunterState.PATROL:
                    {
                        target = GetNearestEnemy();
                        if (WithinRange(transform.position, target.position, agroRange))
                        {
                            SetDestination(target.position);
                            state = HunterState.ATTACK;
                        }
                        else
                        {
                            if (!agent.hasPath || agent.remainingDistance < agent.stoppingDistance)
                            {
                                SetDestination(transform.position + Random.insideUnitSphere * 50f);
                            }
                        }
                        break;
                    }
                case HunterState.ATTACK:
                    {
                        if ((agent.destination - target.position).magnitude > 1f)
                        {
                            SetDestination(target.position);
                            Debug.DrawLine(target.position, target.position + Vector3.up * 100f, Color.black);
                        }

                        if (target.TryGetComponent<SniperAI>(out SniperAI ai))
                        {
                            if (ai.health <= 0)
                            {
                                state = HunterState.PATROL;
                                break;
                            }
                        }
                        else


                        if (target.TryGetComponent<PlayerController>(out PlayerController pc))
                        {
                            if (pc.health <= 0)
                            {
                                state = HunterState.PATROL;
                                break;
                            }
                        }



                        Collider[] colliders = Physics.OverlapSphere(attackBox.transform.position, 5f);

                        bool attackable = false;

                        foreach (Collider c in colliders)
                        {
                            if (c.tag == "PlayerAI" || c.tag == "Player")
                            {
                                attackable = true;
                                break;
                            }
                        }


                        if (attackable)
                        {

                            if (curremtAttackCooldown <= 0)
                            {
                                curremtAttackCooldown = attackCooldown;
                                animator.Play("attack");
                            }
                            agent.ResetPath();
                        }
                        else
                        {
                            SetDestination(target.transform.position);
                        }

                        break;
                    }

            }
        }

        else
        {
            agent.enabled = false;
            animator.Play("death");
            clbox.enabled = false;
            enemyList.Remove(this);

        }


        Debug.DrawLine(agent.destination, agent.destination + Vector3.up * 50, Color.green);
    }




    bool SetDestination(Vector3 position)
    {
        if (agent.enabled)
        {
            bool result = NavMesh.SamplePosition(position, out NavMeshHit hit, 15f, NavMesh.AllAreas);
            if (result)
            {
                result = agent.SetDestination(hit.position);
            }

            return result;

        }

        return false;
    }






    public bool WithinRange(Vector3 position, Vector3 position2, float range)
    {

        return (position - position2).magnitude <= range;

    }

    public SniperAI GetNearestEnemyAI()
    {
        if (SniperAI.sniperList == null)
            return null;
        SniperAI closest = null;
        foreach (SniperAI e in SniperAI.sniperList)
        {

            if (closest == null)
                closest = e;

            if ((e.transform.position - transform.position).sqrMagnitude < (closest.transform.position - transform.position).sqrMagnitude)
            {
                closest = e;
            }
        }

        return closest;
    }
    public Transform GetNearestEnemy()
    {

        Transform closest = player.transform;
        foreach (SniperAI e in SniperAI.sniperList)
        {

            if (closest == null)
                closest = e.transform;

            if ((e.transform.position - transform.position).sqrMagnitude < (closest.transform.position - transform.position).sqrMagnitude)
            {
                closest = e.transform;
            }
        }

        return closest;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Caller:{gameObject.name} Collider: {other.name}");

        if (other.TryGetComponent<Bullet>(out Bullet bullet))
        {
            agent.enabled = false;


            rigidbody.MovePosition(rigidbody.position + bullet.transform.forward * bullet.knockback);


            agent.enabled = true;
            if (other.tag == "playerAI" || other.tag == "player")
            {
                currentHealth -= bullet.damage;
            }

            if (state == HunterState.PATROL)
            { 
                if (other.tag == "playerAI")
                {
                    target = GetNearestEnemyAI().transform;
                    state = HunterState.ATTACK;

                }
                else if (other.tag == "player")
                {
                    target = player.transform;
                    state = HunterState.ATTACK;
                }

                SetDestination(target.position);

            }
        }


    }

}
