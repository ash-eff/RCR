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
    [SerializeField] private TextMeshProUGUI mousePosText;
    [SerializeField] private TextMeshProUGUI playerPosText;
    [SerializeField] private TextMeshProUGUI directiontoMouseText;

    
    private float idleTimer = 10f;
    private PlayerInputs playerInputs;
    private Vector2 directionAxis;
    private Vector3 screenBounds;
    private Vector3 cursorDirection;
    private float cursorDirectionRotation;
    private bool isFiring = false;
    public UnityEvent OnIdleEvent;
    public UnityEvent OnWakeEvent;
    public UnityEvent OnShootEvent;
    public UnityEvent OnSwapEvent; 
    public UnityEvent OnSpecialEvent;

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
        playerInputs.Player.WeaponSwap.performed += cxt => OnSwapEvent.Invoke();
        playerInputs.Player.Shoot.performed += cxt => isFiring = true;
        playerInputs.Player.Shoot.canceled += cxt => isFiring = false;
        playerInputs.Player.SpecialAbility.performed += cxt => OnSpecialEvent.Invoke();
        
        //playerInputs.Player.Shoot.performed += cxt => isFiring = true;
        //playerInputs.Player.Shoot.canceled += cxt => isFiring = false;
        //playerInputs.Player.Reload.performed += cxt => isReloading = true;

        if (OnShootEvent == null) OnShootEvent = new UnityEvent();
        if (OnSpecialEvent == null) OnSpecialEvent = new UnityEvent();
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
        
        if(isFiring)
            OnShootEvent.Invoke();

        AdjustCursorPosition();
        cursorDirectionRotation = MyUtils.GetAngleFromVectorFloat(cursorDirection.normalized);
    }

    private void AdjustCursorPosition()
    {
        var originPos = transform.position;
        var mousePos = MyUtils.GetMouseWorldPositionWithZ();
        var convertedMousePos = new Vector3(mousePos.x, 0, mousePos.z + mousePos.y);
        var aimDirection = (convertedMousePos - originPos).normalized;
        mousePosText.text = "Mouse Position X: " + mousePos.x.ToString("000.00")
                                                 + " Y: " + (mousePos.z + mousePos.y).ToString("000.00");
        playerPosText.text = "Origin Position X: " + originPos.x.ToString("000.00") + " Z: " +
                        originPos.z.ToString("000.00");
        directiontoMouseText.text = "Direction X: " + aimDirection.x.ToString("0.0") + " Y: " + aimDirection.y.ToString("0.0") + " Z: " +
                                    aimDirection.z.ToString("0.0");
        cursor.transform.position = convertedMousePos;
        
        //Debug.DrawLine(originPos, cursor.transform.position, Color.green);
          
        cursorDirection = new Vector2(aimDirection.x, aimDirection.z);

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
