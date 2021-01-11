using System;
using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerCursor : MonoBehaviour
{
    [SerializeField] private bool cameraLookOn = true;
    [SerializeField] private float cursorRadius;
    [SerializeField] private Transform camFollowPoint;
    [SerializeField] private Transform cursor;
    [SerializeField] private SpriteRenderer cursorSprite;
    [Range(.1f, 1f)] [SerializeField] private float cursorSensitivity;
    private PlayerInputs playerInputs;
    private Vector3 aimDirection;
    private Vector3 cursorDirection;

    //public Vector3 GetAimDirection => aimDirection;
    public Vector3 GetCursorDirection => cursorDirection.normalized;
    //public Vector3 GetAimPosition => nonZeroDirection.normalized * cursorRadius;

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }
    
    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerInputs.Player.Aim.performed += cxt => SetAimDirection(cxt.ReadValue<Vector2>());
        playerInputs.Player.Aim.canceled += cxt => ResetAimDirection();
        cursorSprite.enabled = false;
        cursorDirection = Vector3.right;
        Cursor.visible = false;
    }

    private void Update()
    {
        // for controller
        CursorPosition();

        // for mouse
        //MousePosition();
        
        CameraFollowPosition();
    }

    private void CursorPosition()
    {
        var originPos = transform.position;
        var desiredPosition = cursor.position;
        desiredPosition += aimDirection * cursorSensitivity;
        float distance = MyUtils.DistanceBetweenObjects(originPos, desiredPosition);

        if (distance > cursorRadius)
        {
            Vector2 clampedToRad = MyUtils.Direction2D(originPos, desiredPosition);
            clampedToRad *= cursorRadius / distance;
            cursor.position = originPos + MyUtils.Vec2DTo3D(clampedToRad);
        }
        else
        {
            cursor.position = desiredPosition;
        }

        cursorDirection = cursor.position - originPos;
    }

    private void CameraFollowPosition()
    {
        if (cameraLookOn)
        {
            var originPos = transform.position;
            var maxPosition = aimDirection * cursorRadius / 2;
        
            if (aimDirection != Vector3.zero)
            {
                camFollowPoint.position = originPos + maxPosition;
            }
            else
            {
                camFollowPoint.position = originPos;
            }
        }
        else
        {
            camFollowPoint.position = transform.position;
        }
    }

    private void MousePosition()
    {
        Vector2 originPos = transform.position;
        float distance = MyUtils.DistanceBetweenObjects(originPos, aimDirection);

        if (distance > cursorRadius)
        {
            Vector2 clampedToRad = MyUtils.Direction2D(originPos, aimDirection);
            clampedToRad *= cursorRadius / distance;
            cursor.position = transform.position + MyUtils.Vec2DTo3D(clampedToRad);
        }
        else
        {
            cursor.position = aimDirection;
        }
    }

    private void SetAimDirection(Vector2 pos) => aimDirection = pos;
    
    private void ResetAimDirection() => aimDirection = Vector3.zero;
}
