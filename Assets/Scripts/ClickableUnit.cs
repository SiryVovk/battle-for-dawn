using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ClickableUnit : MonoBehaviour
{
    private static float ADD_OFFSET = 0.51f;

    private TileMap map;
    private Unit unit;

    [SerializeField] private GameObject unitTileLight;
    [SerializeField] private GameObject moveUnitLight;
    [SerializeField] private GameObject attackUnitLight;

    public Dictionary<Node, List<Node>> allPathes;


    private void Awake()
    {
        unit = this.gameObject.GetComponent<Unit>();
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<TileMap>();
    }

    public void OnMouseUp()
    {
        map.selectedUnit = this.gameObject;
        map.unitComponent = unit;
        ClearLightAndPathes();
        allPathes = map.AllPathes(unit.UnitX, unit.UnitZ);
        Instantiate(unitTileLight, new Vector3(unit.UnitX, map.map[unit.UnitX, unit.UnitZ].YPos + ADD_OFFSET, unit.UnitZ), Quaternion.Euler(new Vector3(90, 0, 0)));
        LightTiles();

        foreach (Node node in map.graph[unit.UnitX,unit.UnitZ].edges)
        {
            if (map.map[node.xPos, node.zPos].OnTile && map.map[node.xPos, node.zPos].OnTileObject.tag == "Enemy")
            {
                Instantiate(attackUnitLight, new Vector3(node.xPos, map.map[node.xPos, node.zPos].YPos + ADD_OFFSET, node.zPos), Quaternion.Euler(new Vector3(90, 0, 0)));
            }
        }
    }

    public void ClearLightAndPathes()
    {
        if (allPathes != null)
            allPathes = null;
        GameObject[] objects = GameObject.FindGameObjectsWithTag("LightTiles");
        
        for(int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
        }
    }

    private void LightTiles()
    {
        foreach(Node node in allPathes.Keys)
        {
            if (map.map[node.xPos, node.zPos].OnTile && map.map[node.xPos, node.zPos].OnTileObject.tag != "Enemy")
            {
                Instantiate(unitTileLight, new Vector3(node.xPos, map.map[node.xPos, node.zPos].YPos + ADD_OFFSET, node.zPos), Quaternion.Euler(new Vector3(90, 0, 0)));

            }
            else if(!map.map[node.xPos, node.zPos].OnTile)
            {
                Instantiate(moveUnitLight, new Vector3(node.xPos, map.map[node.xPos, node.zPos].YPos + ADD_OFFSET, node.zPos), Quaternion.Euler(new Vector3(90, 0, 0)));
            }
        }
    }
}
