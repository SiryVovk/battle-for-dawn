using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour
{
    [SerializeField] private float tileCost;
    [SerializeField]private bool onTile;
    private int xPos;
    private int zPos;
    private float yPos;

    private void Awake()
    {
        xPos = (int)gameObject.transform.position.x;
        zPos = (int)gameObject.transform.position.z;
        yPos = gameObject.transform.position.y;
    }
    public float TileCost
    {
        get { return tileCost; }
    }

    public int XPos
    {
        get { return xPos; }
    }

    public int ZPos
    {
        get { return zPos; }
    }

    public float YPos
    {
        get { return yPos; }
    }

    public bool OnTile
    {
        get { return onTile; }
        set { onTile = value; }
    }
}
