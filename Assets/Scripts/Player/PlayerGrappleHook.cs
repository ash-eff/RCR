using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using UnityEngine;
using System.Linq;

public class PlayerGrappleHook : MonoBehaviour
{
    [SerializeField] private Transform grappleIndicator;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float grappleRange;
    [SerializeField] private Grapple grapple;
    [SerializeField] private LayerMask whatIsGrappleable;

    private Vector2 aimDir;
    private PlayerInputs playerInputs;
    private Vector2 grapplePoint;
    private Vector2 currentGrapplePosition;
    [SerializeField] private float grappleTime;
    [SerializeField] private bool canGrapple = true;
    [SerializeField] private bool goingOut = false;
    [SerializeField] private bool comingIn = false;

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
        playerInputs.Player.Grapple.performed += cxt => StartGrapple();
        //playerInputs.Player.Shoot.canceled += cxt => StopGrapple();
    }

    // Update is called once per frame
    void Update()
    {
        if (aimDir == Vector2.zero)
            aimDir = transform.right;
        
        SetIndicatorDirection(aimDir);
    }
    
    void LateUpdate() {
        //DrawRope();
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
        if (canGrapple)
        {
            canGrapple = false;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDir.normalized, grappleRange, whatIsGrappleable);
            if (hit) {
                grapplePoint = hit.point;
                Debug.DrawLine(transform.position, grapplePoint, Color.red, 5f);
            }
            else
            {
                grapplePoint = (Vector2)transform.position + aimDir.normalized * grappleRange;
                Debug.DrawLine(transform.position, grapplePoint, Color.blue, 5f);
            }

            StartCoroutine(ShootGrapple(grapplePoint));
        }
    }

    IEnumerator ShootGrapple(Vector2 grappleLocation)
    {
        var rot = MyUtils.GetAngleFromVectorFloat(grappleLocation - (Vector2) transform.position);
        grapple.transform.rotation = Quaternion.Euler(0,0, rot);
        grapple.gameObject.SetActive(true);
        lineRenderer.positionCount = 2;
        var grappleStartPos = grapple.transform.position;
        currentGrapplePosition = grappleStartPos;
        float elapsedTime = 0;
        goingOut = true;
        while (elapsedTime < grappleTime)
        {
            currentGrapplePosition = Vector2.Lerp(currentGrapplePosition, grappleLocation, elapsedTime / grappleTime);
            grapple.transform.position = Vector2.Lerp(grapple.transform.position, grappleLocation, elapsedTime / grappleTime);
            elapsedTime += Time.deltaTime;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, currentGrapplePosition);

            yield return new WaitForEndOfFrame();
        }
        grapple.CheckObjectForGrapple();
        elapsedTime = 0;
        goingOut = false;
        comingIn = true;
        while (elapsedTime < grappleTime)
        {
            currentGrapplePosition = Vector2.Lerp(currentGrapplePosition, grappleStartPos, elapsedTime / grappleTime);
            grapple.transform.position = Vector2.Lerp(grapple.transform.position, grappleStartPos, elapsedTime / grappleTime);
            elapsedTime += Time.deltaTime;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, currentGrapplePosition);

            yield return new WaitForEndOfFrame();
        }
        
        grapple.StopGrapple();
        grapple.gameObject.SetActive(false);
        comingIn = false;
        StopGrapple();
        canGrapple = true;
    }
    
    void StopGrapple() {
        lineRenderer.positionCount = 0;
    }
}
