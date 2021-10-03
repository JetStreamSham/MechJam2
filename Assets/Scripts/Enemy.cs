using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static List<Enemy> enemyList;

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
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Caller:{gameObject.name} Collider: {other.name}");
        if (other.tag == "playerAI" || other.tag == "player")
        {
            currentHealth -= 10;
        }
    }
}
