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
    public static List<SniperAI> sniperList;

    public NavMeshAgent agent;
    public Rigidbody rigidbody;
    public Animator animator;

    public Transform player;
    public Transform home;

    public Enemy target;

    public SNIPER_STATE state;

    public Transform barrel;
    public GameObject bullet;

    public float health = 100;

    public float shootTime;
    public float currentShootTime;


    public bool canReachTarget = false;

    public GameObject explode;


    private void Awake()
    {
        if (sniperList == null)
        {
            sniperList = new List<SniperAI>();
        }

        if (!sniperList.Contains(this))
        {
            sniperList.Add(this);
        }
    }

    void Start()
    {
        PlayerController p = FindObjectOfType<PlayerController>();
        if (p)
            player = p.transform;
        NavMeshHit navHit;
        bool result1 = NavMesh.SamplePosition(transform.position, out navHit, 10f, NavMesh.AllAreas);

        bool result2 = navHit.hit;

        agent.radius += 1;
        agent.radius -= 1;

        if (navHit.hit)
            agent.Warp(navHit.position);

        //Debug.Log(result1 + " " + result2);

        Debug.DrawLine(transform.position, transform.position + Vector3.up * 50, Color.blue);
        Debug.DrawLine(transform.position, transform.position + Vector3.forward * 50, Color.red);
    }


    void EscortDestination()
    {
        Vector3 escortLocation = player.transform.position;
        Vector3 randomLoc = Random.onUnitSphere;
        randomLoc.y = 0;
        escortLocation += 50f * randomLoc.normalized;


        agent.SetDestination(escortLocation);
        Debug.DrawLine(agent.destination, agent.destination + Vector3.up * 50, Color.blue);
    }

    bool CombatDestination()
    {

        Vector3 escortLocation = target.transform.position;
        Vector3 dir = transform.position - target.transform.position;

        dir.y = 0;
        escortLocation += 150f * dir.normalized;


        Debug.DrawLine(agent.destination, agent.destination + Vector3.up * 50, Color.blue);
        return agent.SetDestination(escortLocation);
    }


    public Enemy GetNearestEnemy()
    {
        if (Enemy.enemyList == null)
            return null;
        Enemy closest = null;
        foreach (Enemy e in Enemy.enemyList)
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

    public Enemy GetRandomEnemy()
    {
        return Enemy.enemyList[Random.Range(0, Enemy.enemyList.Count)];
    }


    public bool WithinRange(Vector3 position, Vector3 position2, float range)
    {

        return (position - position2).magnitude <= range;

    }


    void Shoot()
    {
        Debug.Log("Layer:" + animator.layerCount);
        animator.Play("shoot",1);
        currentShootTime = shootTime;

        //shoot em
        GameObject instancedBullet = GameObject.Instantiate(bullet);
        instancedBullet.tag = "playerAI";
        instancedBullet.transform.position = barrel.position + barrel.forward;

        Rigidbody bulletBody = instancedBullet.GetComponent<Rigidbody>();

        Vector3 dir = (target.transform.position + target.transform.up) - barrel.transform.position;
        instancedBullet.GetComponent<Bullet>().damage = 20;

        bulletBody.AddForce(dir.normalized * 25000f);
        Destroy(instancedBullet, 25);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            PlayerController p = FindObjectOfType<PlayerController>();
            if (p)
                player = p.transform;
        }

        if (Input.GetKey(KeyCode.L))
            Explode();
        Debug.DrawLine(agent.destination, agent.destination + Vector3.up * 50, Color.red);
        currentShootTime -= Time.deltaTime;
        if (health > 0)
        {
            switch (state)
            {
                case SNIPER_STATE.ESCORT:
                    {
                        //enemy within firing range;

                        target = GetNearestEnemy();
                        if (target.currentHealth <= 0)
                            target = null;
                        if (target != null)
                        {
                            bool inRange = (WithinRange(target.transform.position, transform.position, 200f));
                            Debug.DrawLine(target.transform.position, target.transform.position + 200 * -(target.transform.position - transform.position).normalized, Color.green);
                            if (inRange)
                            {
                                barrel.LookAt(target.transform);
                                if (currentShootTime <= 0)
                                {
                                    Shoot();
                                }
                            }
                        }
                        // Get within escort distance
                        if (WithinRange(player.position, transform.position, 65f))
                        {
                            agent.ResetPath();
                        }
                        else
                        {
                            EscortDestination();
                        }

                        break;
                    }
                case SNIPER_STATE.COMBAT:
                    {
                        //enemy within firing range;

                        if (target == null)
                            target = GetNearestEnemy();
                        else if (target.currentHealth <= 0)
                            target = null;


                        if (target != null)
                        {

                            //can fire
                            bool inRange = (WithinRange(target.transform.position, transform.position, 400f));
                            Debug.DrawLine(target.transform.position, target.transform.position + 200 * -(target.transform.position - transform.position).normalized, Color.green);
                            if (inRange)
                            {
                                barrel.LookAt(target.transform);
                                if (currentShootTime <= 0)
                                {
                                    Shoot();
                                }
                                agent.ResetPath();
                            }
                            else
                            {
                                CombatDestination();

                            }
                        }

                        break;
                    }
                case SNIPER_STATE.RETREAT:
                    {
                        agent.SetDestination(home.position);
                        break;
                    }
            }


            Vector3 input = transform.InverseTransformDirection(agent.velocity.normalized);

            if (agent.velocity.sqrMagnitude > 0)
            {
                animator.SetBool("isWalking", Mathf.Abs(input.x) > 0 || Mathf.Abs(input.z) > 0);
                animator.SetFloat("horizontal", input.x);
                animator.SetFloat("vertical", input.z);
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetFloat("horizontal", 0);
                animator.SetFloat("vertical", 0);
            }


            Debug.DrawLine(transform.position, transform.position + input * 50, Color.red);
        }
        else
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
        sniperList.Remove(this);
        Destroy(gameObject);

    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Caller:{gameObject.name} Collider: {other.name}");
        if (other.tag == "enemy")
        {
            health -= 30;
        }
    }
    private void LateUpdate()
    {

    }
}
