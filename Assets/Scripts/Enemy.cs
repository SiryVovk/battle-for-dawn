using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int actionPoints = 3;
    private int enemyAttackRange = 1;
    private int enemyAttackPower = 2;
    private int enemyAttackCost = 3;
    private int enemyX;
    private int enemyZ;
    private int maxEnemyActionPoints = 3;
    private int health = 10;
    private float speed = 0.1f;

    public Tile targetObject;

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

    public void MakeMove(TileMap tileMap)
    {
        if (CheckInRange(tileMap.map))
        {
            Attack();
        }
        else
        {
            List<Node> path = tileMap.AStarPath(enemyX, enemyZ, targetObject);
            if(path.Count > 0)
               StartCoroutine(MoveToTarget(path, tileMap.map));
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
                    targetObject.OnTileObject.GetComponent<Unit>().TakeDamage(enemyAttackPower);
                    break;
                case "FirePlace":
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
        ChangeOnTileObject(false, null, map[current.xPos, current.zPos]);

        Vector3 startPoint = new Vector3(current.xPos, map[current.xPos, current.zPos].YPos, current.zPos);
        Vector3 endPoint;
        while (i < path.Count - enemyAttackRange && actionPoints > 0)
        {
            current = path[i];
            if (current == null)
                break;

            endPoint = new Vector3(current.xPos, map[current.xPos, current.zPos].YPos, current.zPos);
            float elapsedTime = 0f;
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

        ChangeOnTileObject(true, this.gameObject, map[current.xPos, current.zPos]);
        Vector3 newPosition = new Vector3(current.xPos, map[current.xPos, current.zPos].YPos, current.zPos);
        this.gameObject.transform.position = newPosition;
        enemyX = current.xPos;
        enemyZ = current.zPos;

        if (CheckInRange(map))
            Attack();
    }

    private void ChangeOnTileObject(bool isOn,GameObject onTile,Tile changeTile)
    {
        changeTile.OnTile = isOn;
        changeTile.OnTileObject = onTile;
    }

}
