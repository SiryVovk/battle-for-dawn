using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToUnit : MonoBehaviour
{
    TileMap map;
    GameObject unit;
    Unit unitComponent;

    private void Awake()
    {
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<TileMap>();
        unit = map.selectedUnit;
        unitComponent = unit.GetComponent<Unit>();
    }

    private void OnMouseDown()
    {
        ClickableUnit click = unit.GetComponent<ClickableUnit>();
        Node lastNode = null;
        int xPos = (int)transform.position.x;
        int zPos = (int)transform.position.z;

        foreach (Node key in click.allPathes.Keys)
        {
            if(key.xPos == xPos && key.zPos == zPos)
            {
                lastNode = key;
                break;
            }

        }

        List<Node> path = click.allPathes[lastNode];
        Node firstNode = path[0];

        foreach (Node key in path)
        {
            if(key != firstNode)
                unitComponent.UnitActionPoints -= (int)map.map[key.xPos, key.zPos].TileCost;
        }

        map.map[firstNode.xPos, firstNode.zPos].OnTile = false;
        map.map[lastNode.xPos, lastNode.zPos].OnTile = true;

        unit.transform.position = new Vector3(lastNode.xPos, map.map[lastNode.xPos, lastNode.zPos].YPos, lastNode.zPos);
        unitComponent.UnitX = lastNode.xPos;
        unitComponent.UnitZ = lastNode.zPos;
        click.ClearLightAndPathes();
    }
}
