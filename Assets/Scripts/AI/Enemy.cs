using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static List<Enemy> enemyList;
    protected PlayerController player;

    public int currentHealth = 100;

    private void Awake()
    {
        if (enemyList == null)
        {
            enemyList = new List<Enemy>();
        }
        if (!enemyList.Contains(this))
        {
            enemyList.Add(this);
        }
        if(player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Caller:{gameObject.name} Collider: {other.name}");
        if (other.tag == "playerAI" || other.tag == "player")
        {
            currentHealth -= 1;
        }
    }
}
