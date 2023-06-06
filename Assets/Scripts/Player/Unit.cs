using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private int unitX;
    [SerializeField] private int unitZ;
    [SerializeField] private int unitActionPoints = 6;
    [SerializeField] private int maxUnitActionPoints = 6;
    [SerializeField] private int attackPower = 5;
    [SerializeField] private int attackActionPoints = 2;
    [SerializeField] private int health = 10;
    [SerializeField] private int attackRange = 1;

    private float unitSpeed = 0.1f;

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
    private void Awake()
    {
        unitX = (int)this.gameObject.transform.position.x;
        unitZ = (int)this.gameObject.transform.position.z;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

}