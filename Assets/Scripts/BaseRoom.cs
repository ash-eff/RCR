using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BaseRoom : MonoBehaviour
{
    [SerializeField] private GameObject topDoor, rightDoor, bottomDoor, leftDoor;
    [SerializeField] private GameObject topDoorSprite, rightDoorSprite, bottomDoorSprite, leftDoorSprite;
    [SerializeField] private Collider2D roomTrigger;
    [SerializeField] private GameObject topWall, rightWall, bottomWall, leftWall;
    [SerializeField] private GameObject fog;
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private GameObject enemySpawnPrefab;
    [SerializeField] private GameObject roomKey;
    [SerializeField] private AudioSource doorAudio;
    private GameManager gameManager;
    public Sprite[] minimapIcons; 
    private RoomManager roomManager;
    public bool top, right, bottom, left;
    public bool hasBeenVisited;
    public bool unlockTop, unlockRight, unlockBottom, unlockLeft, unlockAll;

    public Tilemap Fog => fog.GetComponentInChildren<Tilemap>();
    
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        fog.SetActive(true);
        roomManager = FindObjectOfType<RoomManager>();
    }
    
    private void Start()
    {
        AddDoors(top, right, bottom, left);
        PickMinimapRoom();
    }

    public void AddDoors(bool top, bool right, bool bottom, bool left)
    {
        topDoor.SetActive(top);
        topWall.SetActive(!top);
        rightDoor.SetActive(right);
        rightWall.SetActive(!right);
        bottomDoor.SetActive(bottom);
        bottomWall.SetActive(!bottom);
        leftDoor.SetActive(left);
        leftWall.SetActive(!left);
    }

    public void DisableRoomTrigger(bool isDisabled)
    {
        roomTrigger.enabled = !isDisabled;
    }

    public void UnlockDoors(bool top, bool right, bool bottom, bool left)
    {
        if(!hasBeenVisited)
            doorAudio.Play();

        topDoorSprite.SetActive(!top);
        rightDoorSprite.SetActive(!right);
        bottomDoorSprite.SetActive(!bottom);
        leftDoorSprite.SetActive(!left);
    }

    public void EnableMinimap()
    {
        spr.enabled = true;
    }
    
    void PickMinimapRoom(){ 
        if (top){
            if (bottom){
                if (right){
                    if (left)
                    {
                        spr.sprite = minimapIcons[0];
                    }else{
                        spr.sprite = minimapIcons[1];
                    }
                }else if (left){
                    spr.sprite = minimapIcons[2];
                }else{
                    spr.sprite = minimapIcons[3];
                }
            }else{
                if (right){
                    if (left){
                        spr.sprite = minimapIcons[4];
                    }else{
                        spr.sprite = minimapIcons[5];
                    }
                }else if (left){
                    spr.sprite = minimapIcons[6];
                }else{
                    spr.sprite = minimapIcons[7];
                }
            }
            return;
        }
        if (bottom){
            if (right){
                if(left){
                    spr.sprite = minimapIcons[8];
                }else{
                    spr.sprite = minimapIcons[9];
                }
            }else if (left){
                spr.sprite = minimapIcons[10];
            }else{
                spr.sprite = minimapIcons[11];
            }
            return;
        }
        if (right){
            if (left){
                spr.sprite = minimapIcons[12];
            }else{
                spr.sprite = minimapIcons[13];
            }
        }else{
            spr.sprite = minimapIcons[14];
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemySpawnPrefab, transform.position, Quaternion.identity);
        enemy.GetComponent<EnemySpawn>().spawnedFrom = this;
    }

    public void SpawnKey()
    {
        roomKey.SetActive(true);
        gameManager.RoomUnlocked();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerRoomTrigger"))
        {
            if (!hasBeenVisited)
            {
                Invoke("SpawnEnemy", 1f);
            }
            
            roomManager.CurrentRoom = this;
        }
    }
}
