using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class ItemPickupMessage : MonoBehaviour
{
    [SerializeField] Color32 colorOne = new Color32((byte)22, (byte)133, (byte)248, 255);
    [SerializeField] Color32 colorTwo = new Color32((byte)245, (byte)39, (byte)137, 255);
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private float moveDistance;
    [SerializeField] private float lerpTime;

    void Awake()
    {
        textComponent = GetComponentInChildren<TMP_Text>();
        textComponent.color = colorOne;
        //SetMessage("Kyle is gay.");
    }

    public void SetMessage(string m)
    {
        textComponent.text = m;

        StartCoroutine(FloatText());
    }

    IEnumerator FloatText()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + transform.up * moveDistance;

        float currentLerpTime = 0;

        while (currentLerpTime < lerpTime)
        {
            currentLerpTime += Time.deltaTime;
            var perc = currentLerpTime / lerpTime;
            transform.position = Vector3.Lerp(startPos, endPos, perc);
            textComponent.color = Color.Lerp(colorOne, colorTwo, perc);
            yield return null;
        }
        
        Destroy(this);
    }
}
