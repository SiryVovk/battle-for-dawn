using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    private int actionPoints = 3;
    private int enemyX;
    private int enemyZ;
    private int maxEnemyActionPoints = 3;
    private int health = 10;

    public GameObject targetObject;

    public static Action<Enemy> enemyDie;

    public List<GameObject> toLoockFor = new List<GameObject>(); 


    public int EnemyX
    {
        get { return enemyX; }
        set { enemyX = value; }
    }

    public int EnemyZ
    {
        get { return enemyZ; }
        set { enemyZ = value; }
    }

    public int MaxEnemyActionPoints
    {
        get { return maxEnemyActionPoints; }
    }

    public int ActionPoints
    {
        get { return actionPoints; }
        set { actionPoints = value; }
    }

    private void Awake()
    {
        enemyX = (int)this.gameObject.transform.position.x;
        enemyZ = (int)this.gameObject.transform.position.z;
    }

    public void TakeDamage(int attack)
    {
        health -= attack;

        if(health <= 0)
        {
            Destroy(this.gameObject);
            enemyDie?.Invoke(this);
        }
    }

    public bool InToLookFor(GameObject playerObject)
    {
        foreach(GameObject target in toLoockFor)
        {
            if (target.tag == playerObject.tag)
                return true;
        }

        return false;
    }
}
