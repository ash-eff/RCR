using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] private LayerMask grappleMask;
    [SerializeField] private float checkRadius = .25f;
    private IGrappleable grappleable = null;

    public void CheckObjectForGrapple()
    {
        // circlecast item
        RaycastHit2D hit =
            Physics2D.CircleCast(transform.position, checkRadius, Vector2.right, Mathf.Infinity, grappleMask);

        if (hit)
        {
            // if grapplable, pull back toward you
            grappleable = hit.collider.GetComponent<IGrappleable>();
            if (grappleable != null)
            {
                grappleable.Pull(transform);
            }

            // if wall, cieling or ground, pull player toward it

            // else do nothing
        }
    }

    public void StopGrapple()
    {
        if(grappleable != null) { grappleable.StopPulling(); }
    }
}
