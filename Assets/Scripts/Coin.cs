using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public bool doneBouncing = false;
    public bool isSpinning = false;

    public void DoneBouncing()
    {
        doneBouncing = true;
    }

    public void DoneSpinning()
    {
        isSpinning = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
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
