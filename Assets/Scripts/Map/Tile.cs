using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile
{
    [SerializeField] private string name;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float tileYOffset;

    public float TileYOffset
    {
        get { return tileYOffset; }
    }
    public GameObject GetPrefab
    {
        get { return prefab; }
    }

}
