using System;
using System.Collections;
using System.Collections.Generic;
using Ash.StateMachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyHandler : MonoBehaviour
{
    //IEnumerator TrackPlayerMovement()
    //{
    //    yield return new WaitForSeconds(3f);
    //    Debug.Log("Tracking");
    //    while (true)
    //    {
    //        agent.SetDestination(player.transform.position);
    //        yield return new WaitForSeconds(.5f);
    //    }
    //}

    //takedamage

    //die
    
    //public bool CheckLineOfSightToPlayer()
    //{

    //}


    
    //public void FireWeapon(Vector3 dir, float rot)
    //{
    //    UpdateAmmo();
    //    var offset = Random.Range(-12, 12);
    //    rot += offset;
    //    //Instantiate(shellPrefab, startPosition, quaternion.identity);
    //    Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, rot));
    //    //shakeTimer = .25f;
    //    lastShot = Time.time;
    //}
//
    //public void Reload()
    //{
    //    isReloading = true;
    //    reloadUI.SetActive(true);
    //    ammoAmount = 0;
    //    
    //    StartCoroutine(IeReload());
//
    //    IEnumerator IeReload()
    //    {
    //        int reloadCount = 0;
    //        while (reloadCount < maxAmmo)
    //        {
    //            yield return new WaitForSeconds(.33f);
    //            reloadCount++;
    //        }
//
    //        ammoAmount = maxAmmo;
    //        isReloading = false;
    //        reloadUI.SetActive(false);
    //    }
    //}
    //
    //private void UpdateAmmo()
    //{
    //    if (ammoAmount > 0)
    //    {
    //        ammoAmount--;
    //    }
    //}
    
    //public void Move(Vector2 velocity)
    //{
    //    rigidbody2D.MovePosition(rigidbody2D.position + velocity * Time.fixedDeltaTime);
    //}

    //private void Die()
    //{
    //    //spawnedFromRoom.SpawnKey();
    //    var chance = Random.value;
    //    if (chance <= .2f)
    //    {
    //        Instantiate(ammoPrefab, transform.position, Quaternion.identity);
    //    }
    //    else
    //    {
    //        Instantiate(coinPrefab, transform.position, Quaternion.identity);
    //    }
    //    OnEnemyDeath.Invoke();
    //    GameObject obj = Instantiate(enemyDeathPrefab, transform.position, Quaternion.identity);
    //    Destroy(obj, 1.25f);
    //    Destroy(gameObject);
    //}

}
