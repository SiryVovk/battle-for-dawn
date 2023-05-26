using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MoveToUnit : MonoBehaviour
{
    private static float ADD_OFFSET = 0.51f;

    [SerializeField] private GameObject line;

    private TileMap map;
    private GameObject unit;
    private Unit unitComponent;
    private ClickableUnit click;
    private LineRenderer lineRenderer;

    private List<GameObject> createdObjects = new List<GameObject>();

    [SerializeField] private GameObject attackLightPrefab;

    private void Awake()
    {
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<TileMap>();
        unit = map.selectedUnit;
        unitComponent = unit.GetComponent<Unit>();
        click = unit.GetComponent<ClickableUnit>();
        lineRenderer = line.GetComponent<LineRenderer>();
    }

    private void OnMouseDown()
    {
        Node lastNode = FindNode((int)transform.position.x, (int)transform.position.z);
        List<Node> path = click.allPathes[lastNode];
        Node firstNode = path[0];

        unitComponent.UnitActionPoints -= CostToLastTile(path, firstNode);

        map.map[firstNode.xPos, firstNode.zPos].OnTile = false;
        map.map[lastNode.xPos, lastNode.zPos].OnTile = true;

        map.map[firstNode.xPos, firstNode.zPos].OnTileObject = null;
        map.map[lastNode.xPos, lastNode.zPos].OnTileObject = unit;

        unit.transform.position = new Vector3(lastNode.xPos, map.map[lastNode.xPos, lastNode.zPos].YPos, lastNode.zPos);
        unitComponent.UnitX = lastNode.xPos;
        unitComponent.UnitZ = lastNode.zPos;
        click.ClearLightAndPathes();
        ClearObjects();
        click.OnMouseUp();
    }

    private void OnMouseEnter()
    {
        Node lastNode = FindNode((int)transform.position.x, (int)transform.position.z);
        List<Node> path = click.allPathes[lastNode];
        Node firstNode = path[0];

        if (CostToLastTile(path, firstNode) <= unitComponent.UnitActionPoints + unitComponent.AttackActionPoints)
        {
            foreach(Node node in lastNode.edges)
            {
                if (map.map[node.xPos, node.zPos].OnTile && map.map[node.xPos, node.zPos].OnTileObject.tag == "Enemy")
                {
                    createdObjects.Add(Instantiate(attackLightPrefab, new Vector3(node.xPos, map.map[node.xPos, node.zPos].YPos + ADD_OFFSET, node.zPos), Quaternion.Euler(new Vector3(90, 0, 0))));
                }
            }
        }

        Vector3[] points = new Vector3[path.Count];

        for(int i = 0; i < points.Length;i++)
        {
            float yPos = map.map[path[i].xPos, path[i].zPos].YPos;
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
                cost += (int)map.map[key.xPos, key.zPos].TileCost;
        }

        return cost;
    }
}
