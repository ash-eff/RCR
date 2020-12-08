using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using UnityEngine;
using System.Linq;

public class PlayerGrappleHook : MonoBehaviour
{
    [SerializeField] private Transform grappleIndicator;
    [SerializeField] private LineRenderer lr;
    [SerializeField] private float distModifier;
    [SerializeField] private float grappleRange;
    private Vector2 aimDir;
    private PlayerInputs playerInputs;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip;
    private Vector3 currentGrapplePosition;

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
        aimDir =  transform.right;
	    
        playerInputs = new PlayerInputs();

        playerInputs.Player.Aim.performed += cxt => SetAim(cxt.ReadValue<Vector2>());
        playerInputs.Player.LeftBumper.performed += cxt => ActivateIndicator(true);
        playerInputs.Player.LeftBumper.canceled += cxt => ActivateIndicator(false);
        playerInputs.Player.Shoot.performed += cxt => StartGrapple();
        playerInputs.Player.Shoot.canceled += cxt => StopGrapple();
    }

    // Update is called once per frame
    void Update()
    {
        if (aimDir == Vector2.zero)
            aimDir = transform.right;
        
        
        SetIndicatorDirection(aimDir);
    }
    
    void LateUpdate() {
        DrawRope();
    }
    
    private void SetIndicatorDirection(Vector2 aim)
    {
        var rot = MyUtils.GetAngleFromVectorFloat(aim);
        grappleIndicator.localRotation = Quaternion.Euler(0,0, rot);
    }

    private void ActivateIndicator(bool b)
    {
        grappleIndicator.gameObject.SetActive(b);
    }
    
    private void SetAim(Vector2 aim)
    {
        aimDir = aim;
    }
    
    void StartGrapple()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDir, grappleRange, whatIsGrappleable);
        if (hit) {
            grapplePoint = hit.point;
        }
        else
        {
            grapplePoint = (Vector2)transform.position + aimDir * grappleRange;
        }
        
        
        
        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;
        Debug.DrawLine(currentGrapplePosition, grapplePoint);
    }
    
    void StopGrapple() {
        lr.positionCount = 0;
    }
    
    void DrawRope() {
        //If not grappling, don't draw rope
        if (lr.positionCount == 0) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }
}
