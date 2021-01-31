using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject shellPrefab;
    [SerializeField] private GameObject projectilePrefab;

    public virtual void FireBullet(float rotation, Vector2 gunPosition)
    {
        Debug.Log("Pistol");
        var offset = Random.Range(-4, 4);
        rotation += offset;
        FireProjectile(rotation);
        EjectShell(gunPosition);
    }
    
    public void EjectShell(Vector2 gunPos)
    {
        Instantiate(shellPrefab, gunPos, quaternion.identity);
        Destroy(gameObject);
    }

    public void FireProjectile(float rot)
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, rot));
    }
}
