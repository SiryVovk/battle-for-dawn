using System;
using UnityEngine;

public class FirePlace : MonoBehaviour
{
    [SerializeField] private int maxHealth = 20;

    [SerializeField] private int health;

    [SerializeField] private HealthBarControler healthBar;

    private int xPos;
    private int zPos;

    public static Action firePlaceDie;
    public int XPos
    {
        get { return xPos; }
    }

    public int ZPos
    {
        get { return zPos; }
    }

    private void Awake()
    {
        xPos = (int)transform.position.x;
        zPos = (int)transform.position.z;
        health = maxHealth;
        healthBar.SetMax(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.ChangeHealthBar(health);
        if(health <= 0)
        {
            Destroy(this.gameObject);
            firePlaceDie.Invoke();
        }
    }
}
