using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMessageHandler : MonoBehaviour
{
    [SerializeField] private GameObject floatingText;
    
    public void CreateFloatingText(string message)
    {
        GameObject colObj = Instantiate(floatingText, transform.position, quaternion.identity);
        colObj.GetComponent<CollectionText>().SetMessage(message);
    }
}
