using System;
using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using UnityEngine;

public class PurpleBoss : MonoBehaviour
{
    [SerializeField] private GameObject iris;
    [SerializeField] private SpriteRenderer bodySpr;
    [SerializeField] private SpriteRenderer lidSpr;
    private Material matWhite;
    private Material matDefault;
    private PlayerManager player;
    private Vector2 irisCenterPos;

    private void Awake()
    {
        player = FindObjectOfType<PlayerManager>();
        irisCenterPos = iris.transform.localPosition;
    }
    
    private void Start()
    {
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material; 
        matDefault = bodySpr.material;
    }

    private void Update()
    {
        Vector2 directionToPlayer = player.transform.position - iris.transform.position;
        Debug.DrawLine(iris.transform.position, player.transform.position, Color.red);
        var clampedX = Mathf.Clamp(irisCenterPos.x + directionToPlayer.normalized.x, -.9f, -.1f);
        iris.transform.localPosition = new Vector2(clampedX, -.2f);
    }

    private void TakeDamage(int dmgAmount)
    {
        //health -= dmgAmount;
        //if (health <= 0)
        //{
           // Die();
        //}
    }

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
   //    GameObject obj = Instantiate(enemyDeathPrefab, transform.position, Quaternion.identity);
   //    Destroy(obj, 1.25f);
   //    Destroy(gameObject);
   //    OnEnemyDeath.Invoke();
   //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            TakeDamage(collision.GetComponent<Projectile>().damageAmount);
            Destroy(collision.gameObject);
            
            bodySpr.material = matWhite;
            lidSpr.material = matWhite;
            
            Invoke("SwapMaterialToDefault", .1f);
        }
    }
    
    private void SwapMaterialToDefault()
    {
        bodySpr.material = matDefault;
        lidSpr.material = matDefault;
    }
}
