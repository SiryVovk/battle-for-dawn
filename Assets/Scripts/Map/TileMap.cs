using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    private static int TILE_SIZE = 2;

    [SerializeField]private Tile[] tilesType;

    private TileName[,] map;
    private Node[,] graph;

    [SerializeField]private int xLength;
    [SerializeField]private int zLength;

    [SerializeField] private GameObject selectedUnit;
    private Unit unitComponent;

    private void Awake()
    {
        unitComponent = selectedUnit.GetComponent<Unit>();
        ReadMap();
        GenerateGraphForPathFinding();

    }

    private void ReadMap()
    {
        map = new TileName[xLength, zLength];
        GameObject[] tilesInScene = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject gameObject in tilesInScene)
        {
            int x = (int)gameObject.transform.position.x / TILE_SIZE;
            int z = (int)gameObject.transform.position.z / TILE_SIZE;
            TileName tile = GetTileName(gameObject);
            ClickableTile componentOfTile = gameObject.GetComponent<ClickableTile>();
            componentOfTile.tileX = x;
            componentOfTile.tileZ = z;
            componentOfTile.map = this;
            map[x, z] = tile;
        }
    }

    private void GenerateGraphForPathFinding()
    {
        graph = new Node[xLength, zLength];

        for (int x = 0; x < xLength; x++)
        {
            for (int z = 0; z < zLength; z++)
            {
                graph[x, z] = new Node(x,z);
            }
        }

        for (int x = 0; x < xLength; x++)
        {
            for(int z = 0; z < zLength; z++)
            {
                if(x > 0)
                    graph[x, z].edges.Add(graph[x - 1, z]);
                if (x < xLength - 1)
                    graph[x, z].edges.Add(graph[x + 1,z ]);
                if (z > 0)
                    graph[x, z].edges.Add(graph[x , z - 1]);
                if (z < zLength - 1)
                    graph[x, z].edges.Add(graph[x, z + 1]);
            }
        }
    }

    private TileName GetTileName(GameObject gameObject)
    {
        string name = "";

        foreach(char elemnt in gameObject.name)
        {
            if (!char.IsLetter(elemnt))
                break;
            name += elemnt;
        }

        if (name == "Grass")
            return TileName.Grass;
        else if(name == "HighGround")
            return TileName.HighGround;
        else
            return TileName.Water;
    }

    public void MoveSelectedUnit(int x, int z)
    {
        selectedUnit.transform.position = TileCordToWorldCord(x, z);
        unitComponent.UnitX = x;
        unitComponent.UnitZ = z;
    }

    private Vector3 TileCordToWorldCord(int x, int z)
    {
        return new Vector3(x * TILE_SIZE, tilesType[(int)map[x, z]].TileYOffset, z * TILE_SIZE);
    }
}

enum TileName
{
    Grass,
    HighGround,
    Water
}

