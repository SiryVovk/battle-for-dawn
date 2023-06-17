using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControll : MonoBehaviour
{
    [SerializeField] private AudioClip walck;
    [SerializeField] private AudioClip attack;
    [SerializeField] private AudioClip getHit;

    private AudioSource source;

    private float soundPower = 5;

    GameObject unitAttach;

    private void OnEnable()
    {
        MoveToUnit.isWalking += WalkSound;
        AttackTile.isAttacking += AttackSound;
        Unit.unitTackDamage += GetHitSound;
        Enemy.isAttacking += AttackSound;
        Enemy.isWalking += WalkSound;
        Enemy.tackingDamage += GetHitSound;
    }

    private void OnDisable()
    {
        MoveToUnit.isWalking -= WalkSound;
        AttackTile.isAttacking -= AttackSound;
        Unit.unitTackDamage -= GetHitSound;
        Enemy.isAttacking -= AttackSound;
        Enemy.isWalking -= WalkSound;
        Enemy.tackingDamage -= GetHitSound;
    }
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        unitAttach = this.gameObject.transform.parent.gameObject;
        source.volume = soundPower;
    }

    private void WalkSound(GameObject unit,bool isWalcking)
    {
        if (unit == unitAttach)
        {
            source.clip = walck;
            if(isWalcking)
                source.Play();
            else
                source.Stop();
        }
    }

    private void AttackSound(GameObject unit)
    {
        if (unit == unitAttach)
        {
            source.PlayOneShot(attack);
        }
    }

    private void GetHitSound(GameObject unit)
    {
        if (unit == unitAttach)
        {
            source.PlayOneShot(getHit);
        }
    }
}
