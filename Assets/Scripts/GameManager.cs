using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        GridSystem grid = new GridSystem(10, 10, 2);
    }
}
