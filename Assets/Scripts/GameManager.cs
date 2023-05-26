using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private List<Unit> allPlayerUnits = new List<Unit>();
    private List<Enemy> allEnemys = new List<Enemy>();
    private int turnNumber = 1;

    private void Awake()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        
        foreach (GameObject unit in units)
        {
            allPlayerUnits.Add(unit.GetComponent<Unit>());
        }
    }

    public void EndTurn()
    {
        foreach (Unit unit in allPlayerUnits)
        {
            unit.UnitActionPoints = unit.MaxUnitActionPoints;
        }

        turnNumber++;
    }

}
