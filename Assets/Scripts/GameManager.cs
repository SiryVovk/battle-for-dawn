using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private HashSet<Unit> allPlayerUnits = new HashSet<Unit>();
    private HashSet<Enemy> allEnemys = new HashSet<Enemy>();
    private List<GameObject> allPlayersObject = new List<GameObject>();

    private TileMap tileMap;
    private Tile[] bordersOfMap;

    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject endCanvas;

    [SerializeField] private TextMeshProUGUI turns;

    [SerializeField]private List<GameObject> enemysPref;

    private int turnNumber = 0;
    private int spawnRate = 1;
    private int spawnTurns = 3;
    private bool isGameOver = false;

    private void OnEnable()
    {
        Enemy.enemyDie += EnemyDie;
        FirePlace.firePlaceDie += GameOver;
    }

    private void OnDisable()
    {
        Enemy.enemyDie -= EnemyDie;
        FirePlace.firePlaceDie -= GameOver;
    }

    private void Start()
    {
        tileMap = GameObject.FindGameObjectWithTag("Map").GetComponent<TileMap>();
        int numberOfTiles = tileMap.map.GetLength(0) * 2 + tileMap.map.GetLength(1) * 2 - 4;
        bordersOfMap = new Tile[numberOfTiles];

        FillBordersArray();

        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

        foreach (GameObject unit in units)
        {
            allPlayerUnits.Add(unit.GetComponent<Unit>());
            allPlayersObject.Add(unit);
        }

        GameObject fire = GameObject.FindGameObjectWithTag("FirePlace");
        allPlayersObject.Add(fire);
    }

    public void EndTurn()
    {
        foreach (Unit unit in allPlayerUnits)
        {
            unit.UnitActionPoints = unit.MaxUnitActionPoints;
            unit.gameObject.GetComponent<ClickableUnit>().ClearLightAndPath();
        }

        ClearLights();
        tileMap.selectedUnit = null;

        if (!isGameOver)
        {
            foreach (Enemy enemy in allEnemys)
            {
                enemy.ActionPoints = enemy.MaxEnemyActionPoints;
                FindClosestEnemyGoal(enemy);
                enemy.MakeMove(tileMap);
            }

            turnNumber++;
            turns.text = turnNumber.ToString();

            if (turnNumber % spawnTurns == 0)
            {
                SpawnEnemys();
            }
        }
    }

    private void FillBordersArray()
    {
        int i = 0;

        for (int x = 0; x < tileMap.map.GetLength(0); x++)
        {
            for (int z = 0; z < tileMap.map.GetLength(1); z++)
            {
                if (x == 0 || x == tileMap.map.GetLength(0) - 1)
                {
                    bordersOfMap[i] = tileMap.map[x, z];
                    i++;
                }
                else if (z == 0 || z == tileMap.map.GetLength(1) - 1)
                {
                    bordersOfMap[i] = tileMap.map[x, z];
                    i++;
                }
            }
        }
    }

    private Vector3 ChoseSpawnPosition()
    {
        int borderTilesCount = bordersOfMap.Length;
        int[] numbers = GenerateDistinctNumbers(0, borderTilesCount - 1);

        for(int i = 0; i < borderTilesCount - 1; i++)
        {
            Tile currTile = bordersOfMap[numbers[i]];
            if (!currTile.OnTile)
            {
                return new Vector3(currTile.XPos, currTile.YPos, currTile.ZPos);
            }
        }
        
        return new Vector3();
    }

    private static int[] GenerateDistinctNumbers(int minValue, int maxValue)
    {
        int[] numbers = new int[maxValue - minValue + 1];

        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = minValue + i;
        }

        System.Random random = new System.Random();

        for (int i = numbers.Length - 1; i >= 1; i--)
        {
            int j = random.Next(i + 1);
            int temp = numbers[j];
            numbers[j] = numbers[i];
            numbers[i] = temp;
        }

        return numbers;
    }

    private void ClearLights()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("LightTiles");

        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
        }
    }

    private void FindClosestEnemyGoal(Enemy enemy)
    {
        float minDistance = Mathf.Infinity;
        foreach(GameObject unit in allPlayersObject)
        {
            if(enemy.InToLookFor(unit))
            {
                float dist = MinimalDistanse(enemy, unit);
                if(minDistance > dist)
                {
                    minDistance = dist;
                    enemy.targetObject = tileMap.map[(int)unit.transform.position.x, (int)unit.transform.position.z];
                }
            }
        }
    }

    private float MinimalDistanse(Enemy enemy,GameObject unit)
    {
        int xPos = (int)unit.transform.position.x;
        int zPos = (int)unit.transform.position.z;

       return Mathf.Sqrt((xPos - enemy.EnemyX) * (xPos - enemy.EnemyX) + (zPos - enemy.EnemyZ) * (zPos - enemy.EnemyZ));

    }

    private void SpawnEnemys()
    {
        int spawned = 0;
        while (spawned < spawnRate)
        {
            Vector3 spawnPos = ChoseSpawnPosition();
            GameObject enemy = Instantiate(enemysPref[0], spawnPos, Quaternion.identity);
            allEnemys.Add(enemy.GetComponent<Enemy>());
            tileMap.map[(int)spawnPos.x, (int)spawnPos.z].OnTile = true;
            tileMap.map[(int)spawnPos.x, (int)spawnPos.z].OnTileObject = enemy;
            spawned++;
        }
    }

    private void EnemyDie(Enemy enemy)
    {
        allEnemys.Remove(enemy);
    }

    private void GameOver()
    {
        isGameOver = true;
       foreach(GameObject playerUnit in allPlayersObject)
       {
            playerUnit.GetComponent<Collider>().enabled = false;
       }

        gameCanvas.active = false;
        endCanvas.active = true;

        endCanvas.GetComponent<EndScreen>().SetScore(turnNumber);
    }
}
