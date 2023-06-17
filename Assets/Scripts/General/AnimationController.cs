using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator anim;
    private int isWalkingHash;
    private int isAttackingHash;
    private int tackingDamageHash;
    private GameObject parentGameObject;

    private void OnEnable()
    {
        MoveToUnit.isWalking += PlayWalkingAnimation;
        AttackTile.isAttacking += PlayAttack;
        Unit.unitTackDamage += PlayTackDamage;
        Enemy.isAttacking += PlayAttack;
        Enemy.isWalking += PlayWalkingAnimation;
        Enemy.tackingDamage += PlayTackDamage;
    }

    private void OnDisable()
    {
        MoveToUnit.isWalking -= PlayWalkingAnimation;
        AttackTile.isAttacking -= PlayAttack;
        Unit.unitTackDamage -= PlayTackDamage;
        Enemy.isAttacking -= PlayAttack;
        Enemy.isWalking -= PlayWalkingAnimation;
        Enemy.tackingDamage -= PlayTackDamage;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isAttackingHash = Animator.StringToHash("isAttacking");
        tackingDamageHash = Animator.StringToHash("tackingDamage");
        parentGameObject = this.gameObject.transform.parent.gameObject;
    }

    private void PlayWalkingAnimation(GameObject unitToPlay, bool isPlaying)
    {
        if(unitToPlay == parentGameObject)
            anim.SetBool(isWalkingHash, isPlaying);
    }

    private void PlayAttack(GameObject unitToPlay)
    {
        if(unitToPlay == parentGameObject)
            anim.SetBool(isAttackingHash, true);
    }

    private void PlayTackDamage(GameObject unitToPlay)
    {
        if(unitToPlay == parentGameObject)
            anim.SetBool(tackingDamageHash, true);
    }

    public void AttackAnimEnd()
    {
        anim.SetBool(isAttackingHash, false);
    }

    public void TackDamageAnimEnd()
    {
        anim.SetBool(tackingDamageHash, false);
    }
}
