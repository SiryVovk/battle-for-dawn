using System.Collections.Generic;
using UnityEngine;

public class ClickableUnit : MonoBehaviour
{
    private static float ADD_OFFSET = 0.51f;

    private TileMap tileMap;
    private Unit unit;

    [SerializeField] private GameObject unitTileLight;
    [SerializeField] private GameObject moveUnitLight;
    [SerializeField] private GameObject attackUnitLight;

    public Dictionary<Node, List<Node>> allPathes;

    private void Start()
    {
        unit = this.gameObject.GetComponent<Unit>();
        tileMap = GameObject.FindGameObjectWithTag("Map").GetComponent<TileMap>();
    }

    public void OnMouseUp()
    {
        tileMap.selectedUnit = this.gameObject;
        tileMap.unitComponent = unit;
        ClearLightAndPath();
        allPathes = tileMap.AllPathes(unit.UnitX, unit.UnitZ);
        Instantiate(unitTileLight, new Vector3(unit.UnitX, tileMap.map[unit.UnitX, unit.UnitZ].YPos + ADD_OFFSET, unit.UnitZ), Quaternion.Euler(new Vector3(90, 0, 0)),this.transform);
        LightTiles();

        if (unit.UnitActionPoints >= unit.AttackActionPoints)
        {
            InAttackRange();
        }
    }

    public  void ClearLightAndPath()
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
        Tile[,] map = tileMap.map;
        foreach(Node node in allPathes.Keys)
        {
            if (map[node.xPos, node.zPos].OnTile && map[node.xPos, node.zPos].OnTileObject.tag != "Enemy")
            {
                Instantiate(unitTileLight, new Vector3(node.xPos, map[node.xPos, node.zPos].YPos + ADD_OFFSET, node.zPos), Quaternion.Euler(new Vector3(90, 0, 0)));
            }
            else if(!map[node.xPos, node.zPos].OnTile)
            {
                Instantiate(moveUnitLight, new Vector3(node.xPos, map[node.xPos, node.zPos].YPos + ADD_OFFSET, node.zPos), Quaternion.Euler(new Vector3(90, 0, 0)));
            }
        }
    }

    private void InAttackRange()
    {
        int distanceCheck = 1;
        Tile[,] map = tileMap.map;
        int xLength = map.GetLength(0);
        int zLength = map.GetLength(1);
        int unitX = unit.UnitX;
        int unitZ = unit.UnitZ;
        while (distanceCheck <= unit.AttackRange)
        {
            string targetTag = "Enemy";
            if (unitX + distanceCheck < xLength && map[unitX + distanceCheck, unitZ].OnTile && map[unitX + distanceCheck, unitZ].OnTileObject.tag == targetTag)
                Instantiate(attackUnitLight, new Vector3(unitX + distanceCheck, map[unitX + distanceCheck, unitZ].YPos + ADD_OFFSET, unit.UnitZ), Quaternion.Euler(new Vector3(90, 0, 0)), this.transform);
            if (unitX - distanceCheck >= 0 && map[unitX - distanceCheck, unitZ].OnTile && map[unitX - distanceCheck, unitZ].OnTileObject.tag == targetTag)
                Instantiate(attackUnitLight, new Vector3(unitX - distanceCheck, map[unitX - distanceCheck, unitZ].YPos + ADD_OFFSET, unit.UnitZ), Quaternion.Euler(new Vector3(90, 0, 0)), this.transform);
            if (unitZ + distanceCheck < zLength && map[unitX, unitZ + distanceCheck].OnTile && map[unitX, unitZ + distanceCheck].OnTileObject.tag == targetTag)
                Instantiate(attackUnitLight, new Vector3(unitX, map[unitX, unitZ + distanceCheck].YPos + ADD_OFFSET, unit.UnitZ + distanceCheck), Quaternion.Euler(new Vector3(90, 0, 0)), this.transform);
            if (unitZ - distanceCheck >= 0 && map[unitX, unitZ - distanceCheck].OnTile && map[unitX, unitZ - distanceCheck].OnTileObject.tag == targetTag)
                Instantiate(attackUnitLight, new Vector3(unitX, map[unitX, unitZ - distanceCheck].YPos + ADD_OFFSET, unit.UnitZ - distanceCheck), Quaternion.Euler(new Vector3(90, 0, 0)), this.transform);
            distanceCheck++;
        }
    }

}
