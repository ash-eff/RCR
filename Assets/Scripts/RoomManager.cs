using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Color fogOpaque;
    [SerializeField] private Color fogHalfOpaque;
    [SerializeField] private Color fogTransparent;
    [SerializeField] private BaseRoom currentRoom;
    [SerializeField] private BaseRoom lastRoom;


    public BaseRoom CurrentRoom
    {
        get => currentRoom;
        set
        {
            if (currentRoom != null)
            {
                lastRoom = currentRoom;
                lastRoom.DisableRoomTrigger(false);
                StartCoroutine(ChangeColor(lastRoom.Fog, fogTransparent, fogHalfOpaque));
            }
            else
            {
                lastRoom = null;
            }

            currentRoom = value;
            
            if (currentRoom.hasBeenVisited)
            {
                StartCoroutine(ChangeColor(currentRoom.Fog, fogHalfOpaque, fogTransparent));
                currentRoom.UnlockDoors(true, true, true, true );

            }
            else
            {
                StartCoroutine(ChangeColor(currentRoom.Fog, fogOpaque, fogTransparent));
                currentRoom.UnlockDoors(false, false, false, false );
                currentRoom.EnableMinimap();
            }

            currentRoom.hasBeenVisited = true;
            currentRoom.DisableRoomTrigger(true);
        } 
    }

    IEnumerator ChangeColor(Tilemap _tileMap, Color fromColor, Color toColor)
    {
        var duration = 1f;
        var currentTime = 0f;
        var startAlpha = fromColor.a;
        var endAlpha = toColor.a;
        var currentColor = new Color(fromColor.r, fromColor.g, fromColor.b, startAlpha);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            var currentAlpha = Mathf.Lerp(startAlpha, endAlpha, currentTime);
            currentColor = new Color(fromColor.r, fromColor.g, fromColor.b, currentAlpha);
            _tileMap.color = currentColor;
            yield return null;
        }
    }
}
