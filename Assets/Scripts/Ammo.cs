using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ammo : MonoBehaviour
{
    [SerializeField] private GameObject collectionText;
    private int ammoAmount = 0;
    private PlayerManager player;
    private void Awake()
    {
        ammoAmount = Random.Range(3, 10);
        player = FindObjectOfType<PlayerManager>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject collectionObj = Instantiate(collectionText, other.transform.position, quaternion.identity);
            collectionObj.GetComponent<CollectionText>().SetMessage("+"+ ammoAmount);
            player.CollectAmmo(ammoAmount);
            Destroy(gameObject);
        }
    }
}
