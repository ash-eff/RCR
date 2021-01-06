using System;
using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerCursor : MonoBehaviour
{
    [SerializeField] private float cursorRadius;
    [SerializeField] private Transform cursor;
    [SerializeField] private SpriteRenderer cursorSprite;
    private PlayerInputs playerInputs;
    private Vector3 aimDirection;
    private Vector3 nonZeroDirection;

    public Vector3 GetAimDirection => aimDirection;
    public Vector3 GetNonZeroDirection => nonZeroDirection.normalized;
    public Vector3 GetAimPosition => nonZeroDirection.normalized * cursorRadius;

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
        nonZeroDirection = Vector3.right;
        Cursor.visible = false;
    }

    private void Update()
    {
        // for controller
        CursorPosition();
        
        // for mouse
        //MousePosition();
    }

    private void CursorPosition()
    {
        if (aimDirection != Vector3.zero)
        {
            nonZeroDirection = aimDirection;
        }
        
        cursor.position = transform.position + GetAimPosition;
    }

    private void MousePosition()
    {
        Vector2 originPos = transform.position;
        float distance = MyUtils.DistanceBetweenObjects(originPos, GetAimDirection);

        if (distance > cursorRadius)
        {
            Vector2 clampedToRad = MyUtils.Direction2D(originPos, GetAimDirection);
            clampedToRad *= cursorRadius / distance;
            cursor.position = transform.position + MyUtils.Vec2DTo3D(clampedToRad);
        }
        else
        {
            cursor.position = GetAimDirection;
        }
    }

    private void SetAimDirection(Vector2 pos) => aimDirection = pos;
    
    private void ResetAimDirection() => aimDirection = Vector3.zero;
}
