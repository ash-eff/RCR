using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMoneyHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    private PlayerMessageHandler messageHandler;
    private int coinTotal;

    private void Awake()
    {
        messageHandler = GetComponent<PlayerMessageHandler>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CoinPickUp"))
        {
            CollectCoin(other.GetComponent<Coin>().CollectCoin());
        }
    }

    private void CollectCoin(int coinAmount)
    {
        coinTotal += coinAmount;
        coinText.text = "$" + coinTotal.ToString();
        messageHandler.CreateFloatingText("$" + coinAmount.ToString());
    }
}
