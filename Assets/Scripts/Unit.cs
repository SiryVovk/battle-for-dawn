using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private int unitX;
    [SerializeField] private int unitZ;

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
}