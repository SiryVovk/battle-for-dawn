using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Node
{
    public List<Node> edges;
    public int xPos;
    public int zPos;

    public Node(int x, int z)
    {
        edges = new List<Node>();
        xPos = x;
        zPos = z;
    }
}
