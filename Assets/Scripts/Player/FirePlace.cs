using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlace : MonoBehaviour
{
    private int health = 100;
    private int xPos;
    private int zPos;

    private int XPos
    {
        get { return xPos; }
    }

    private int ZPos
    {
        get { return zPos; }
    }

    private void Awake()
    {
        xPos = (int)transform.position.x;
        zPos = (int)transform.position.z;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
