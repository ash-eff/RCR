using System;
using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Transform cursor;
    [SerializeField] private float edgeOffsetX, edgeOffsetY;
    
    private float idleTimer = 10f;
    private PlayerInputs playerInputs;
    private Vector2 directionAxis;
    private Vector3 screenBounds;
    private Vector3 cursorDirection;
    private float cursorDirectionRotation;
    public UnityEvent OnJumpEvent;
    public UnityEvent OnLandEvent;
    public UnityEvent OnIdleEvent;
    public UnityEvent OnWakeEvent;
    public UnityEvent OnShootEvent;
    public UnityEvent OnSwapEvent;



    //public UnityEvent OnFallEvent;
    //public UnityEvent OnDashEvent;
    
    private bool isIdle;
    public Vector2 GetDirectionAxis => directionAxis;
    public Vector3 GetCursorDirection => cursorDirection.normalized;
    public float GetRotationToCursor => cursorDirectionRotation;
    public bool CheckIsIdle => isIdle;

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
        //Cursor.visible = false;
        playerInputs = new PlayerInputs();
        playerInputs.Player.Move.performed += cxt => SetMovement(cxt.ReadValue<Vector2>());
        playerInputs.Player.Move.canceled += cxt => ResetMovement();
        playerInputs.Player.Jump.started += cxt => OnJumpStart();
        playerInputs.Player.Jump.performed += cxt => OnHeldJump();
        playerInputs.Player.Jump.canceled += cxt => OnLand();
        playerInputs.Player.WeaponSwap.performed += cxt => OnSwapEvent.Invoke();
        playerInputs.Player.Shoot.performed += cxt => OnShootEvent.Invoke();


        //playerInputs.Player.Shoot.performed += cxt => isFiring = true;
        //playerInputs.Player.Shoot.canceled += cxt => isFiring = false;
        //playerInputs.Player.Reload.performed += cxt => isReloading = true;
        if (OnJumpEvent == null) OnJumpEvent = new UnityEvent();
        if (OnLandEvent == null) OnLandEvent = new UnityEvent();
        //if (OnFallEvent == null) OnFallEvent = new UnityEvent();
        //if (OnDashEvent == null) OnDashEvent = new UnityEvent();
        cursorDirection = Vector3.right;
        UpdateScreenBounds();
    }

    private void Update()
    {
        // add more checks here, for instance id the player isn't shooting or jumping
        if (directionAxis == Vector2.zero)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0 && !isIdle)
            {
                isIdle = true;
                OnIdleEvent.Invoke();
            }
        }
        else if(isIdle)
        {
            isIdle = false;
            OnWakeEvent.Invoke();
        }
        else
        {
            idleTimer = 10f;
        }
        
        //if (isFiring && Time.time > rateOfFire + lastShot && ammoAmount > 0)
        //{
        //    OnShootEvent.Invoke();
        //}
        
        AdjustCursorPosition();
        cursorDirectionRotation = MyUtils.GetAngleFromVectorFloat(cursorDirection.normalized);
    }

    private void AdjustCursorPosition()
    {
        var originPos = transform.position;
        var mousePos = MyUtils.GetMouseWorldPosition();
        var aimDirection = mousePos - originPos;
        cursor.transform.position = mousePos;
        cursorDirection = aimDirection;

        Vector2 clampedPos = cursor.transform.position;
        clampedPos.x = Mathf.Clamp(cursor.transform.position.x, originPos.x - screenBounds.x, originPos.x + screenBounds.x);
        clampedPos.y = Mathf.Clamp(cursor.transform.position.y, originPos.y - screenBounds.y, originPos.y + screenBounds.y);
        cursor.transform.position = clampedPos;
    }
    
    private void UpdateScreenBounds()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        screenBounds = new Vector2((width / 2) + edgeOffsetX, (height / 2) + edgeOffsetY);
    }
    
    private void SetMovement(Vector2 movement) => directionAxis = movement;
	
    private void ResetMovement() => directionAxis = Vector3.zero;
	
    private void OnJumpStart()
    {
        OnJumpEvent.Invoke();
    } 
	
    private void OnHeldJump()
    {
    }

    private void OnLand()
    {
        OnLandEvent.Invoke();
    }
}
