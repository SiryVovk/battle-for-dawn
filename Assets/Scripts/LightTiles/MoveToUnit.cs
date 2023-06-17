using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToUnit : MonoBehaviour
{
    private static float ADD_OFFSET = 0.51f;

    [SerializeField] private GameObject line;

    private TileMap tileMap;
    private GameObject unit;
    private Unit unitComponent;
    private ClickableUnit click;
    private LineRenderer lineRenderer;

    private List<GameObject> createdObjects = new List<GameObject>();

    [SerializeField] private GameObject attackLightPrefab;

    public static Action<GameObject, bool> isWalking; 

    private void Awake()
    {
        tileMap = GameObject.FindGameObjectWithTag("Map").GetComponent<TileMap>();
        unit = tileMap.selectedUnit;
        unitComponent = unit.GetComponent<Unit>();
        click = unit.GetComponent<ClickableUnit>();
        lineRenderer = line.GetComponent<LineRenderer>();
    }

    private void OnMouseDown()
    {
        ClearObjects();
        Node lastNode = FindNode((int)transform.position.x, (int)transform.position.z);
        List<Node> path = click.allPathes[lastNode];
        Node firstNode = path[0];
        Tile[,] map = tileMap.map;

        unitComponent.UnitActionPoints -= CostToLastTile(path, firstNode);

        map[firstNode.xPos, firstNode.zPos].OnTile = false;
        map[lastNode.xPos, lastNode.zPos].OnTile = true;

        map[firstNode.xPos, firstNode.zPos].OnTileObject = null;
        map[lastNode.xPos, lastNode.zPos].OnTileObject = unit;
        unitComponent.UnitX = lastNode.xPos;
        unitComponent.UnitZ = lastNode.zPos;

        EnableTile();
        StartCoroutine(MoveToLastTile(path, map));
    }

    private void OnMouseEnter()
    {
        Node lastNode = FindNode((int)transform.position.x, (int)transform.position.z);
        List<Node> path = click.allPathes[lastNode];
        Node firstNode = path[0];
        Tile[,] map = tileMap.map;

        if (CostToLastTile(path, firstNode) <= unitComponent.UnitActionPoints + unitComponent.AttackActionPoints)
        {
            foreach(Node node in lastNode.edges)
            {
                if (map[node.xPos, node.zPos].OnTile && map[node.xPos, node.zPos].OnTileObject.tag == "Enemy")
                {
                    createdObjects.Add(Instantiate(attackLightPrefab, new Vector3(node.xPos, map[node.xPos, node.zPos].YPos + ADD_OFFSET, node.zPos), Quaternion.Euler(new Vector3(90, 0, 0))));
                }
            }
        }

        Vector3[] points = new Vector3[path.Count];

        for(int i = 0; i < points.Length;i++)
        {
            float yPos = map[path[i].xPos, path[i].zPos].YPos;
            points[i] = new Vector3(path[i].xPos, yPos, path[i].zPos);
        }

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);

        createdObjects.Add(Instantiate(line, new Vector3(0, 1, 0), Quaternion.identity));
    }

    private void OnMouseExit()
    {
        ClearObjects();
    }

    private void ClearObjects()
    {
        foreach (GameObject obj in createdObjects)
        {
            Destroy(obj);
        }

        createdObjects = new List<GameObject>();
    }

    private void EnableTile()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("LightTiles");

        foreach(GameObject obj in objects) 
        {
            Collider col;
            obj.TryGetComponent<Collider>(out col);
            if(col != null)
                col.enabled = false;
            obj.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private Node FindNode(int xPos, int zPos)
    {
        foreach (Node key in click.allPathes.Keys)
        {
            if (key.xPos == xPos && key.zPos == zPos)
            {
                return key;
            }
        }
        return null;
    }

    private int CostToLastTile(List<Node> path, Node firstNode)
    {
        int cost = 0;

        foreach (Node key in path)
        {
            if (key != firstNode)
                cost += (int)tileMap.map[key.xPos, key.zPos].TileCost;
        }

        return cost;
    }

    private IEnumerator MoveToLastTile(List<Node> path, Tile[,] map)
    {
        Node current = path[0];
        Vector3 startPoint = new Vector3(current.xPos, map[current.xPos, current.zPos].YPos, current.zPos);
        Vector3 endPoint;
        for (int i = 1; i < path.Count; i++)
        {
            current = path[i];
            endPoint = new Vector3(current.xPos, map[current.xPos, current.zPos].YPos, current.zPos);
            unitComponent.MoveDirection = ChoseMoveDirection(startPoint,endPoint);
            isWalking.Invoke(unit, true);
            float elapsedTime = 0f;
            while (elapsedTime < unitComponent.UnitSpeed)
            {
                unit.transform.position = Vector3.Lerp(startPoint, endPoint, unitComponent.UnitSpeed / elapsedTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            unit.transform.position = endPoint;
            yield return null;
            startPoint = endPoint;
        }

        isWalking.Invoke(unit, false);
        unit.transform.position = startPoint;
        click.ClearLightAndPath();
        click.OnMouseUp();
    }

    private MoveDirection ChoseMoveDirection(Vector3 start, Vector3 end)
    {
        if(start.x > end.x)
            return MoveDirection.Left;
        if(start.x < end.x)
            return MoveDirection.Right;
        if (start.z > end.z)
            return MoveDirection.Down;
        if (start.z < end.z)
            return MoveDirection.Up;

        return unitComponent.MoveDirection;
    }
}
