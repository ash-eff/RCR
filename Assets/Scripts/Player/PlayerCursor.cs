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
    [Range(.1f, 1f)] [SerializeField] private float cursorSensitivity;
    [SerializeField] private Vector2 edgeOffset;
    private PlayerInputs playerInputs;
    private Vector3 aimDirection;
    private Vector3 cursorDirection;
    private Vector3 screenBounds;

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
        //playerInputs.Player.Aim.performed += cxt => SetAimDirection(cxt.ReadValue<Vector2>());
        //playerInputs.Player.Aim.canceled += cxt => ResetAimDirection();
        //cursorSprite.enabled = false;
        cursorDirection = Vector3.right;
        //Cursor.visible = false;
        UpdateScreenBounds();
    }

    private void Update()
    {
        // for controller
        //CursorPosition();

        // for mouse
        MousePosition();
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
    

    private void MousePosition()
    {
        var originPos = transform.position;
        var mousePos = MyUtils.GetMouseWorldPosition();
        aimDirection = mousePos - originPos;
        cursor.position = mousePos;
        cursorDirection = aimDirection;

        Vector2 clampedPos = cursor.position;
        clampedPos.x = Mathf.Clamp(cursor.position.x, originPos.x - screenBounds.x, originPos.x + screenBounds.x);
        clampedPos.y = Mathf.Clamp(cursor.position.y, originPos.y - screenBounds.y, originPos.y + screenBounds.y);
        cursor.position = clampedPos;
    }

    private void UpdateScreenBounds()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        screenBounds = new Vector2((width / 2) + edgeOffset.x, (height / 2) + edgeOffset.y);
    }

    private void SetAimDirection(Vector2 pos) => aimDirection = pos;
    
    private void ResetAimDirection() => aimDirection = Vector3.zero;
}
