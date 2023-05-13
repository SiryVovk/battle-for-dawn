using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int width;
    private int hight;
    private int cellSize;

    private int[,] grid;

    public GridSystem(int width, int hight, int cellSize)
    {
        this.width = width;
        this.hight = hight;
        this.cellSize = cellSize;

        grid = new int[width, hight];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < hight; y++)
            {
                UtilsClass.CreateWorldText(x + "," + y,null,GetWorldPos(x,y) + new Vector3(cellSize,cellSize) * 0.5f,10,Color.white,TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x, y + 1),Color.red,100f);
                Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x + 1, y), Color.red, 100f);
            }
        }

        Debug.DrawLine(GetWorldPos(0, hight), GetWorldPos(width, hight), Color.red, 100f);
        Debug.DrawLine(GetWorldPos(width, 0), GetWorldPos(width, hight), Color.red, 100f);
    }

    private Vector3 GetWorldPos(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize;
    }
}
