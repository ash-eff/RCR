using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapRoom : MonoBehaviour
{
    public GameObject roomUp, roomDown, roomRight, roomLeft, 
                       roomUpDown, roomRightLeft, roomUpRight, roomUpLeft, 
                       roomDownRight, roomDownLeft, roomUpLeftDown, roomRightUpLeft, 
                       roomDownRightUp, roomLeftDownright, roomUpDownLeftRight;
    public Tilemap fogTiles;
    private RoomManager roomManager;
    public bool up, down, left, right;
    public bool hasBeenVisited;
    public int type;

    private void Awake()
    {
        roomManager = FindObjectOfType<RoomManager>();
    }

    private void Start()
    {
        PickRoom();
    }

    void PickRoom(){ 
        if (up){
            if (down){
                if (right){
                    if (left){
                        roomUpDownLeftRight.SetActive(true);
                    }else{
                        roomDownRightUp.SetActive(true);
                    }
                }else if (left){
                    roomUpLeftDown.SetActive(true);
                }else{
                    roomUpDown.SetActive(true);
                }
            }else{
                if (right){
                    if (left){
                        roomRightUpLeft.SetActive(true);
                    }else{
                        roomUpRight.SetActive(true);
                    }
                }else if (left){
                    roomUpLeft.SetActive(true);
                }else{
                    roomUp.SetActive(true);
                }
            }
            return;
        }
        if (down){
            if (right){
                if(left){
                    roomLeftDownright.SetActive(true);
                }else{
                    roomDownRight.SetActive(true);
                }
            }else if (left){
                roomDownLeft.SetActive(true);
            }else{
                roomDown.SetActive(true);
            }
            return;
        }
        if (right){
            if (left){
                roomRightLeft.SetActive(true);
            }else{
                roomRight.SetActive(true);
            }
        }else{
            roomLeft.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Room Trigger");
            roomManager.CurrentRoom = this;
        }
    }
}
