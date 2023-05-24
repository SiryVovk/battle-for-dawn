using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private int unitX;
    [SerializeField] private int unitZ;
    [SerializeField] private int unitActionPoints = 6;
    [SerializeField] private int maxUnitActionPoints = 6;
    private bool unitActive = true;

    public TileMap map;

    public int UnitX
    {
        get => unitX;
        set => unitX = value;
    }

    public int UnitZ
    {
        get => unitZ;
        set => unitZ = value;
    }

    public int UnitActionPoints
    {
        get { return unitActionPoints;}
        set { unitActionPoints = value; }
    }

    public int MaxUnitActionPoints
    {
        get { return maxUnitActionPoints; }
    }

    public bool UnitActive
    {
        get { return unitActive; }
        set { unitActive = value; }
    }

    private void Awake()
    {
        unitX = (int)this.gameObject.transform.position.x;
        unitZ = (int)this.gameObject.transform.position.z;
    }

}