using System;
using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using TMPro;
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
    public float angleToCursor;
    private bool isFiring = false;
    public UnityEvent OnIdleEvent;
    public UnityEvent OnWakeEvent;
    public UnityEvent OnShootEvent;
    public UnityEvent OnSwapEvent; 
    public UnityEvent OnSpecialEvent;

    private bool isIdle = true;
    public Vector2 GetDirectionAxis => directionAxis;
    public Vector3 GetCursorDirection => cursorDirection.normalized;
    public float GetAngleToCursor => angleToCursor;
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
        playerInputs.Player.WeaponSwap.performed += cxt => OnSwapEvent.Invoke();
        playerInputs.Player.Shoot.performed += cxt => isFiring = true;
        playerInputs.Player.Shoot.canceled += cxt => isFiring = false;
        playerInputs.Player.SpecialAbility.performed += cxt => OnSpecialEvent.Invoke();
        
        //playerInputs.Player.Shoot.performed += cxt => isFiring = true;
        //playerInputs.Player.Shoot.canceled += cxt => isFiring = false;
        //playerInputs.Player.Reload.performed += cxt => isReloading = true;

        if (OnShootEvent == null) OnShootEvent = new UnityEvent();
        if (OnSpecialEvent == null) OnSpecialEvent = new UnityEvent();
        cursorDirection = Vector3.forward;
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

        if (isFiring)
        {
            if (isIdle)
            {
                isIdle = false;
                OnWakeEvent.Invoke();
            }
            
            OnShootEvent.Invoke();
        }
            

        AdjustCursorPosition();
        angleToCursor = MyUtils.GetAngleFromVectorFloat3D(cursorDirection.normalized);
    }

    private void AdjustCursorPosition()
    {
        var originPos = transform.position;
        var mousePos = MyUtils.GetMouseWorldPositionWithZ();
        var convertedMousePos = new Vector3(mousePos.x, 0, mousePos.z + mousePos.y);
        var aimDirection = (convertedMousePos - originPos).normalized;

        cursor.transform.position = convertedMousePos;
        cursorDirection = new Vector3(aimDirection.x, 0f, aimDirection.z);
        ;
        //Vector3 clampedPos = cursor.transform.position;
        //clampedPos.x = Mathf.Clamp(cursor.transform.position.x, originPos.x - screenBounds.x, originPos.x + screenBounds.x);
        //clampedPos.z = Mathf.Clamp(cursor.transform.position.z, originPos.z - screenBounds.y, originPos.z + screenBounds.y);
        //cursor.transform.position = clampedPos;
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
}
