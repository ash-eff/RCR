using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weapon;
    [SerializeField] private GameObject collectionText;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject collectionObj = Instantiate(collectionText, transform.position, quaternion.identity);
            collectionText.GetComponent<CollectionText>().SetMessage(weapon.name);
            other.GetComponent<PlayerManager>().CollectNewWeapon(weapon);
            Destroy(gameObject);
        }
    }
}
