using System;
using UnityEngine;

public class FirePlace : MonoBehaviour
{
    [SerializeField]private int health = 100;
    private int xPos;
    private int zPos;

    public static Action firePlaceDie;
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
        if(health <= 0)
        {
            Destroy(this.gameObject);
            firePlaceDie.Invoke();
        }
    }
}
