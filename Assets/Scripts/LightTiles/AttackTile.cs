using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTile : MonoBehaviour
{
    TileMap tileMap;

    private void Awake()
    {
        tileMap = GameObject.FindGameObjectWithTag("Map").GetComponent<TileMap>();
    }
    private void OnMouseDown()
    {
        int xPos = (int)transform.position.x;
        int zPos = (int)transform.position.z;
        Enemy enemy = tileMap.map[xPos, zPos].OnTileObject.GetComponent<Enemy>();
        Unit  unit = this.GetComponentInParent<Unit>();
        enemy.TakeDamage(unit.AttackPower);
        ClickableUnit clickable = this.GetComponentInParent<ClickableUnit>();
        clickable.OnMouseUp();
        Destroy(gameObject);
    }
}
