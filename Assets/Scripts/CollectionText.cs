using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionText : MonoBehaviour
{
    [SerializeField] private TextMeshPro message;

    private void Awake()
    {
        Destroy(gameObject, 1f);
    }

    public void SetMessage(string m)
    {
        message.text = m;
    }
}
