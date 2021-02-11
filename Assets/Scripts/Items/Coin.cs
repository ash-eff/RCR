using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour
{
    [SerializeField] private GameObject coinText;
    [SerializeField] private Animator anim;
    [SerializeField] private int minAmount, maxAmount;
    private int coinAmount;
    public bool doneBouncing = false;
    public bool isSpinning = false;

    private void Awake()
    {
        coinAmount = Random.Range(minAmount, maxAmount + 1);
    }

    public void DoneBouncing()
    {
        doneBouncing = true;
    }

    public void DoneSpinning()
    {
        isSpinning = false;
    }

    public int CollectCoin()
    {
        Destroy(gameObject);
        return coinAmount;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (doneBouncing)
        {
            if (other.gameObject.CompareTag("PlayerBullet"))
            {
                if (!isSpinning)
                {
                    isSpinning = true;
                    anim.SetTrigger("Spin");
                }
            }
        }
    }
}
