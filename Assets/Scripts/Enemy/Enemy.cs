using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]private int actionPoints = 3;
    [SerializeField]private int enemyAttackRange = 1;
    [SerializeField] private int enemyAttackPower = 2;
    [SerializeField] private int enemyAttackCost = 3;
    [SerializeField] private int maxEnemyActionPoints = 3;
    [SerializeField] private int maxHealth = 10;

    private int health;

    private int enemyX;
    private int enemyZ;
    private float speed = 0.1f;

    private Transform enemyTransform;

    public Tile targetObject;

    public static Action<Enemy> enemyDie;
    public static Action<GameObject,bool> isWalking;
    public static Action<GameObject> isAttacking;
    public static Action<GameObject> tackingDamage;

    [SerializeField]private List<GameObject> toLoockFor = new List<GameObject>();

    private MoveDirection moveDirection = MoveDirection.Down;

    [SerializeField] private HealthBarControler healthBar;
    [SerializeField] private GameObject visualObject;


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

    public MoveDirection MoveDirection
    {
        get { return moveDirection; }
        set
        {
            moveDirection = value;
            ChangeRotation();
        }
    }

    private void Start()
    {
        enemyX = (int)this.gameObject.transform.position.x;
        enemyZ = (int)this.gameObject.transform.position.z;

        enemyTransform = this.gameObject.transform;
        health = maxHealth;
        healthBar.SetMax(maxHealth);
    }

    public void TakeDamage(int attack)
    {
        health -= attack;
        healthBar.ChangeHealthBar(health);
        tackingDamage.Invoke(this.gameObject);

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

    public void MakeMove(TileMap tileMap)
    {
        if (CheckInRange(tileMap.map))
        {
            Attack();
        }
        else
        {
            List<Node> path = tileMap.AStarPath(enemyX, enemyZ, targetObject);
            if (path.Count > 0)
            {
                ChangePos(path, tileMap.map);
                StartCoroutine(MoveToTarget(path, tileMap.map));
            }
        }
    }

    private bool CheckInRange(Tile[,] map)
    {
        int distanceCheck = 1;
        int xLength = map.GetLength(0);
        int zLength = map.GetLength(1);
        while(distanceCheck <= enemyAttackRange)
        {
            string targetTag = targetObject.OnTileObject.tag;
            if (enemyX + distanceCheck < xLength && map[enemyX + distanceCheck, enemyZ].OnTile && map[enemyX + distanceCheck, enemyZ].OnTileObject.tag == targetTag)
                return true;
            if (enemyX - distanceCheck > 0 && map[enemyX - distanceCheck, enemyZ].OnTile && map[enemyX - distanceCheck, enemyZ].OnTileObject.tag == targetTag)
                return true;
            if (enemyZ + distanceCheck < zLength && map[enemyX , enemyZ + distanceCheck].OnTile && map[enemyX, enemyZ + distanceCheck].OnTileObject.tag == targetTag)
                return true;
            if (enemyZ - distanceCheck > 0 && map[enemyX, enemyZ - distanceCheck].OnTile && map[enemyX,enemyZ - distanceCheck].OnTileObject.tag == targetTag)
                return true;
            distanceCheck++;
        }

        return false;
    }

    private void Attack()
    {
        while(actionPoints >= enemyAttackCost)
        {

            switch(targetObject.OnTileObject.tag)
            {
                case "Unit":
                    isAttacking.Invoke(this.gameObject);
                    ChoseAttackDirection(targetObject.OnTileObject);
                    targetObject.OnTileObject.GetComponent<Unit>().TakeDamage(enemyAttackPower);
                    break;
                case "FirePlace":
                    isAttacking.Invoke(this.gameObject);
                    ChoseAttackDirection(targetObject.OnTileObject);
                    targetObject.OnTileObject.GetComponent<FirePlace>().TakeDamage(enemyAttackPower);
                    break;
                default:
                    Debug.Log("Error");
                    break;
            }
            actionPoints -= enemyAttackCost;
        }
    }

    private IEnumerator MoveToTarget(List<Node> path, Tile[,] map)
    {
        int i = 1;
        Node current = path[0];

        Vector3 startPoint = new Vector3(current.xPos, map[current.xPos, current.zPos].YPos, current.zPos);
        Vector3 endPoint;
        while (i < path.Count - enemyAttackRange && actionPoints > 0)
        {
            current = path[i];
            if (current == null)
                break;

            endPoint = new Vector3(current.xPos, map[current.xPos, current.zPos].YPos, current.zPos);
            float elapsedTime = 0f;
            MoveDirection = ChoseMoveDirection(startPoint, endPoint);
            isWalking.Invoke(this.gameObject, true);
            while (elapsedTime < speed)
            {
                this.gameObject.transform.position = Vector3.Lerp(startPoint, endPoint, speed / elapsedTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            actionPoints -= (int)map[current.xPos, current.zPos].TileCost;
            transform.position = endPoint;
            yield return null;
            startPoint = endPoint;
            i++;
        }
        isWalking.Invoke(this.gameObject, false);

        Vector3 newPosition = new Vector3(current.xPos, map[current.xPos, current.zPos].YPos, current.zPos);
        this.gameObject.transform.position = newPosition;
        enemyX = current.xPos;
        enemyZ = current.zPos;

        if (CheckInRange(map))
            Attack();
    }

    private void ChangePos(List<Node> path, Tile[,] map)
    {
        int i = 1;
        int allActionsPoint = actionPoints;
        Node current = path[0];
        ChangeOnTileObject(false, null, map[current.xPos, current.zPos]);

        while (i < path.Count - enemyAttackRange && allActionsPoint > 0)
        {
            current = path[i];
            if (current == null)
                break;
            allActionsPoint -= (int)map[current.xPos, current.zPos].TileCost;
            i++;
        }

        ChangeOnTileObject(true, this.gameObject, map[current.xPos, current.zPos]);
    }

    private void ChangeOnTileObject(bool isOn,GameObject onTile,Tile changeTile)
    {
        changeTile.OnTile = isOn;
        changeTile.OnTileObject = onTile;
    }

    private void ChangeRotation()
    {
        visualObject.transform.rotation = Quaternion.Euler(enemyTransform.rotation.x, (int)moveDirection, enemyTransform.rotation.z);
    }

    private MoveDirection ChoseMoveDirection(Vector3 start, Vector3 end)
    {
        if (start.x > end.x)
            return MoveDirection.Right;
        if (start.x < end.x)
            return MoveDirection.Left;
        if (start.z > end.z)
            return MoveDirection.Up;
        if (start.z < end.z)
            return MoveDirection.Down;

        return MoveDirection;
    }

    private MoveDirection ChoseAttackDirection(GameObject toAttack)
    {
        int toAttackXPos = (int)toAttack.transform.position.x;
        int toAttackZPos = (int)toAttack.transform.position.z;

        if (EnemyX > toAttackXPos)
            return MoveDirection.Left;
        if (EnemyX < toAttackXPos)
            return MoveDirection.Right;
        if (EnemyZ > toAttackZPos)
            return MoveDirection.Down;
        if (EnemyZ < toAttackZPos)
            return MoveDirection.Up;

        return MoveDirection;
    }
}
