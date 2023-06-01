using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public Tile[,] map;
    public Node[,] graph;

    [SerializeField]private int xLength;
    [SerializeField]private int zLength;

    public GameObject selectedUnit;
    public Unit unitComponent;

    private void Awake()
    {
        ReadMap();
        GenerateGraphForPathFinding();
    }

    private void OnEnable()
    {
        Enemy.enemyDie += EnemyDie;
    }

    private void OnDesble()
    {
        Enemy.enemyDie -= EnemyDie;
    }

    private void ReadMap()
    {
        map = new Tile[xLength, zLength];
        GameObject[] tilesInScene = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject gameObject in tilesInScene)
        {
            Tile tile = gameObject.GetComponent<Tile>();
            map[tile.XPos, tile.ZPos] = tile;
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

    public Dictionary<Node, List<Node>> AllPathes(int x, int z)
    {
        Dictionary<Node, float> score = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
        HashSet<Node> toCheck = new HashSet<Node>();
        HashSet<Node> toGo = new HashSet<Node>();

        score[graph[x, z]] = 0;
        toCheck.Add(graph[x,z]);

        while (toCheck.Count > 0)
        {
            Node current = NodeWithLowestScore(toCheck,score);
            toCheck.Remove(current);

            if (current == null)
                break;

            foreach(Node node in current.edges)
            {
                if (!map[node.xPos, node.zPos].OnTile)
                {
                    if (!score.ContainsKey(node))
                    {
                        score[node] = Mathf.Infinity;
                        toCheck.Add(node);
                        prev[node] = null;
                    }

                    float minCost = score[current] + CostToTile(node);

                    if (minCost <= unitComponent.UnitActionPoints && minCost < score[node])
                    {
                        if (!toGo.Contains(node))
                            toGo.Add(node);
                        score[node] = minCost;
                        prev[node] = current;
                    }
                }
            }
        }

        Dictionary<Node, List<Node>> result = new Dictionary<Node, List<Node>>();
        foreach(Node node in toGo)
        {
            result[node] = ReconstructPath(prev, node);
        }

        return result;
    }

    public List<Node> AStarPath(int x, int z, Tile goal)
    {
        HashSet<Node> openSet = new HashSet<Node>();
        HashSet<Node> closeSet = new HashSet<Node>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
        Dictionary<Node, float> gScore = new Dictionary<Node, float>();
        Dictionary<Node, float> fScore = new Dictionary<Node, float>();

        Node start = graph[x, z];
        Node end = graph[goal.XPos, goal.ZPos];

        openSet.Add(start);
        gScore[start] = 0;
        fScore[start] = Heuristic(start, end);

        while (openSet.Count != 0)
        {
            Node current = NodeWithLowestScore(openSet, fScore);

            if (current == end)
                return ReconstructPath(prev, current);

            openSet.Remove(current);
            closeSet.Add(current);

            foreach(Node node in current.edges)
            {
                if (closeSet.Contains(node) || map[node.xPos, node.zPos].OnTile)
                    continue;

                float tentativeGScore = gScore[current] + CostToTile(node);

                if(tentativeGScore < GetGScore(gScore,node))
                {
                    prev[node] = current;
                    gScore[node] = tentativeGScore;
                    fScore[node] = gScore[node] + Heuristic(node, end);

                    if (!openSet.Contains(node))
                        openSet.Add(node);
                }

            }
        }

        return new List<Node>();

    }

    private float Heuristic(Node start, Node goal)
    {
        float xDistToMake = Math.Abs(goal.xPos - start.xPos);
        float zDistToMake = Math.Abs(goal.zPos - start.zPos);
        return xDistToMake + zDistToMake;
    }
    private Node NodeWithLowestScore(HashSet<Node> toCheck, Dictionary<Node, float> score)
    {
        Node lowestNode = null;
        float lowestScore = Mathf.Infinity;
        foreach(Node node in toCheck)
        {
            if(lowestScore > score[node])
            {
                lowestScore = score[node];
                lowestNode = node;
            }
        }

        return lowestNode;
    }

    private float GetGScore(Dictionary<Node,float> gScore, Node node)
    {
        if (gScore.ContainsKey(node))
            return gScore[node];

        gScore[node] = Mathf.Infinity;
        return Mathf.Infinity;
    }

    private List<Node> ReconstructPath(Dictionary<Node,Node> parent,Node current)
    {
        List<Node> path = new List<Node>() { current };
        Node lastNode = current;
        while(parent.ContainsKey(current))
        {
            current = parent[current];
            path.Add(current);
        }

        path.Reverse();
        return path;
    }    

    public float CostToTile(Node neighborn)
    {
        return map[neighborn.xPos,neighborn.zPos].TileCost;
    }

    public Vector3 TileCordToWorldCord(int x, int z)
    {
        return new Vector3(x , map[x,z].YPos, z);
    }

    private void EnemyDie(Enemy enemy)
    {
        map[enemy.EnemyX, enemy.EnemyZ].OnTile = false;
        map[enemy.EnemyX, enemy.EnemyZ].OnTileObject = null;
    }

}

