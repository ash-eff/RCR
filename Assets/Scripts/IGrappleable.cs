using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrappleable
{
    void Pull(Transform withObject);
    void StopPulling();

}
