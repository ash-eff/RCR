using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class PlayerHazardTrigger : MonoBehaviour
{
    [SerializeField] private Collider2D fallTrigger;
    [SerializeField] private Hazard hazard = null;
    [SerializeField] private float hazardGracePeriod;
    private bool inHazard = false;
    
    public UnityEvent OnInHazardEvent;
    public UnityEvent OnOutOfHazardEvent;

    public Hazard GetCurrentHazard => hazard;

    private void Awake()
    {
        if (OnInHazardEvent == null) OnInHazardEvent = new UnityEvent();
        if (OnOutOfHazardEvent == null) OnOutOfHazardEvent = new UnityEvent();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Hazard"))
            if (!inHazard)
            {
                hazard = other.GetComponent<Hazard>();
                inHazard = true;
                StartCoroutine(HazardGracePeriodTimer());
            }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Hazard"))
        {
            hazard = null;
            inHazard = false;
            OnOutOfHazardEvent.Invoke();
        }
    }

    IEnumerator HazardGracePeriodTimer()
    {
        yield return new WaitForSeconds(hazardGracePeriod);
        if (inHazard)
            OnInHazardEvent.Invoke();
    }
}
