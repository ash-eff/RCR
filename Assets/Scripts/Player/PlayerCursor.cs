using System;
using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    [SerializeField] private float cursorRadius;
    [SerializeField] private Transform cursor;
    [Range(1, 10)][SerializeField] private int sensitivity;
    private PlayerInputs playerInputs;
    public Vector2 aimDirection;
    public Vector3 modifiedAim;

    public Vector2 AimDirection => aimDirection;
    public Vector3 GetCursorPosition => cursor.position;
    
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

        Cursor.visible = false;
    }

    private void Update()
    {
        CursorPosition();
    }

    private void CursorPosition()
    {
        modifiedAim = (Vector3) aimDirection / sensitivity;
        var currentPos = cursor.position += modifiedAim;
        //Vector2 mousePos = Camera.main.ScreenToWorldPoint(currentPos);
        Vector2 originPos = transform.position;
        float distance = MyUtils.DistanceBetweenObjects(originPos, currentPos);
        if (distance > cursorRadius)
        {
            Vector2 clampedToRad = MyUtils.Direction2D(originPos, currentPos);
            clampedToRad *= cursorRadius / distance;
            cursor.position = transform.position + MyUtils.Vec2DTo3D(clampedToRad);
        }
        else
        {
            cursor.position = currentPos;
        }
    }

    private void SetAimDirection(Vector2 pos) => aimDirection = pos;
    
    private void ResetAimDirection() => aimDirection = Vector3.zero;
}
