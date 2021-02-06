using System;
using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ObjectiveArrow : MonoBehaviour
{
    //private PlayerManager playerManager;
    private SpriteRenderer arrowSprite;
    private Transform arrowTransform;
    public Transform objective;

    private void Awake()
    {
        //playerManager = GetComponentInParent<PlayerManager>();
        arrowSprite = GetComponentInChildren<SpriteRenderer>();
        arrowTransform = arrowSprite.transform;
    }
    

    public Transform SetObjective
    {
        get => arrowTransform;
        set => arrowTransform = value.transform;
    }

    private void Update()
    {
        if (objective == null)
        {
            arrowSprite.enabled = false;
            return;
        }

        if(arrowSprite.enabled == false)
            arrowSprite.enabled = true;
        
        var angle = MyUtils.GetAngleFromVectorFloat3D(objective.position - transform.position);
        Debug.DrawLine(transform.position, objective.position, Color.red);
        var spriteRot = MyUtils.GetSpriteXYRotationFromZAngle(angle);
        transform.localRotation = Quaternion.Euler(0,0,angle);
        arrowTransform.transform.localRotation = Quaternion.Euler(spriteRot.x, spriteRot.y, 0f);
    }
}
