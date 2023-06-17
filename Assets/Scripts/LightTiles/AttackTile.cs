using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTile : MonoBehaviour
{
    TileMap tileMap;

    public static Action<GameObject> isAttacking;
    private void Awake()
    {
        tileMap = GameObject.FindGameObjectWithTag("Map").GetComponent<TileMap>();
    }
    private void OnMouseDown()
    {
        int xPos = Convert.ToInt32(transform.position.x);
        int zPos = Convert.ToInt32(transform.position.z);
        Enemy enemy = tileMap.map[xPos, zPos].OnTileObject.GetComponent<Enemy>();
        Unit  unit = this.GetComponentInParent<Unit>();
        ChoseAttackDirection(unit, enemy);
        isAttacking.Invoke(this.gameObject.transform.parent.gameObject);
        enemy.TakeDamage(unit.AttackPower);
        ClickableUnit clickable = this.GetComponentInParent<ClickableUnit>();
        clickable.OnMouseUp();
        Destroy(gameObject);
    }

    private void ChoseAttackDirection(Unit playerUnit, Enemy enemy)
    {
        if (playerUnit.UnitX > enemy.EnemyX)
            playerUnit.MoveDirection = MoveDirection.Left;
        if (playerUnit.UnitX < enemy.EnemyX)
            playerUnit.MoveDirection = MoveDirection.Right;
        if (playerUnit.UnitZ > enemy.EnemyZ)
            playerUnit.MoveDirection = MoveDirection.Down;
        if (playerUnit.UnitZ < enemy.EnemyZ)
            playerUnit.MoveDirection = MoveDirection.Up;
    }
}
