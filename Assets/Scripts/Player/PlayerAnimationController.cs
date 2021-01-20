using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer characterSprite;
    [SerializeField] private Animator anim;
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private static readonly int XDir = Animator.StringToHash("xDir");
    private static readonly int YDir = Animator.StringToHash("yDir");

    public void SetSpriteFacingDirection(Vector2 direction)
    {
        anim.SetFloat(XDir, direction.x);
        anim.SetFloat(YDir, direction.y);
    }

    public  void SpriteFlip(float xDir)
    {
        if (xDir > 0)
            characterSprite.transform.localScale = new Vector3(1,1,1);

        if (xDir < 0)
            characterSprite.transform.localScale = new Vector3(-1,1,1);
    }

    public void IsPlayerMoving(Vector3 movement)
    {
        if(movement == Vector3.zero)
            anim.SetBool(Moving, false);
        else
            anim.SetBool(Moving, true);

    }

    public void OnIdleAnimation()
    {
        anim.SetBool(Idle, true);
    }
    
    public void OnWakeAnimation()
    {
        anim.SetBool(Idle, false);
    }

    public void OnJumpAnimation()
    {
        anim.SetBool(Jumping, true);
    }
    
    public void OnLandAnimation()
    {
        anim.SetBool(Jumping, false);
    }
}
