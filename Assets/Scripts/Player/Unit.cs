using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Unit : MonoBehaviour
{
    [SerializeField] private int maxUnitActionPoints = 6;
    [SerializeField] private int attackPower = 5;
    [SerializeField] private int attackActionPoints = 2;
    [SerializeField] private int attackRange = 1;
    [SerializeField] private int maxHealth = 10;

    private int unitX;
    private int unitZ;
    private int unitActionPoints = 6;
    private int health = 10;

    private float unitSpeed = 0.1f;

    private Transform unitTransform;

    [SerializeField] private HealthBarControler healthBar;
    [SerializeField] private GameObject unitVisual;

    private MoveDirection moveDirection = MoveDirection.Down;

    public static Action<GameObject> unitDie;
    public static Action<GameObject> unitTackDamage;
    public int UnitX
    {
        get => unitX;
        set => unitX = value;
    }

    public int UnitZ
    {
        get => unitZ;
        set => unitZ = value;
    }

    public int UnitActionPoints
    {
        get { return unitActionPoints; }
        set { unitActionPoints = value; }
    }

    public int MaxUnitActionPoints
    {
        get { return maxUnitActionPoints; }
    }

    public int AttackPower
    {
        get 
        {
            unitActionPoints -= attackActionPoints;
            return attackPower; 
        }
    }

    public int AttackActionPoints
    {
        get { return attackActionPoints; }
    }

    public int AttackRange
    {
        get { return attackRange; }
    }

    public float UnitSpeed
    {
        get { return unitSpeed; }
    }

    public MoveDirection MoveDirection
    {
        get { return moveDirection; }
        set 
        { 
            moveDirection = value;
            ChangeRotation();
        }
    }
    private void Awake()
    {
        unitTransform = this.gameObject.transform;
        unitX = (int)unitTransform.position.x;
        unitZ = (int)unitTransform.position.z;
        ChangeRotation();
        health = maxHealth;
        healthBar.SetMax(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.ChangeHealthBar(health);
        unitTackDamage.Invoke(this.gameObject);
        if (health <= 0)
        {
            unitDie.Invoke(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void ChangeRotation()
    {
        unitVisual.transform.rotation = Quaternion.Euler(unitTransform.rotation.x, (int)moveDirection, unitTransform.rotation.z);
    }
}

public enum MoveDirection
{
    Up = 0,
    Right = 90,
    Left = 270,
    Down = 180
};