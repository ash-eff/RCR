using System;
using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject shellPrefab;
    [SerializeField] private GameObject projectilePrefab;

    public virtual void FireBullet(float rotation, Vector3 gunPosition)
    {
        var offset = Random.Range(-4, 4);
        rotation += offset;
        FireProjectile(rotation);
        //EjectShell(gunPosition);
    }
    
    public void FireProjectile(float rot)
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0,-rot, 0));
    }
    
    public void EjectShell(Vector3 gunPos)
    {
        Instantiate(shellPrefab, gunPos, quaternion.identity);
        Destroy(gameObject);
    }
}
