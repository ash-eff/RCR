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
    [SerializeField] private SpriteRenderer arrowSprite;
    private Transform arrowTransform;
    private Transform objective;

    private void Awake()
    {
        arrowTransform = arrowSprite.transform;
    }

    public Transform SetObjective
    {
        get => objective;
        set => objective = value.transform;
    }

    private void Update()
    {
        if (objective != null)
        {
            if (MyUtils.DistanceBetweenObjects(objective.position, transform.position) > 4f)
                arrowSprite.enabled = true;
            else
                arrowSprite.enabled = false;
            
            var angle = MyUtils.GetAngleFromVectorFloat3D(objective.position - transform.position);
            var spriteRot = MyUtils.GetSpriteXYRotationFromZAngle(angle);
            transform.localRotation = Quaternion.Euler(0,0,angle);
            arrowTransform.transform.localRotation = Quaternion.Euler(spriteRot.x, spriteRot.y, 0f);
        }
    }
}
