using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int actionPoints = 3;
    private int enemyX;
    private int enemyZ;
    private int maxEnemyActionPoints = 3;
    private int attackPower = 2;
    private int attackCost = 2;
    private int health = 10;
    private int attackRange = 1;

    private void Awake()
    {
        enemyX = (int)this.gameObject.transform.position.x;
        enemyZ = (int)this.gameObject.transform.position.x;
    }

    public void TakeDamage(int attack)
    {
        health -= attack;

        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
