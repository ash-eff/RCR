using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MoveableWorldObject : MonoBehaviour, IGrappleable
{
    public void Pull(Transform withObject)
    {
        transform.parent = withObject;
    }

    public void StopPulling()
    {
        transform.parent = null;
    }
}
