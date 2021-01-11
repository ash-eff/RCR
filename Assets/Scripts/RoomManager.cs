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
    [SerializeField] private MapRoom currentRoom;
    [SerializeField] private MapRoom lastRoom;


    public MapRoom CurrentRoom
    {
        get => currentRoom;
        set
        {
            if (currentRoom != null)
            {
                lastRoom = currentRoom;
                StartCoroutine(ChangeColor(lastRoom.fogTiles, fogTransparent, fogHalfOpaque));
            }
            else
            {
                lastRoom = null;
            }

            currentRoom = value;
            
            if (currentRoom.hasBeenVisited)
            {
                StartCoroutine(ChangeColor(currentRoom.fogTiles, fogHalfOpaque, fogTransparent));

            }
            else
            {
                StartCoroutine(ChangeColor(currentRoom.fogTiles, fogOpaque, fogTransparent));
            }

            currentRoom.hasBeenVisited = true;
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
            currentTime += Time.deltaTime / duration;
            var currentAlpha = Mathf.Lerp(startAlpha, endAlpha, currentTime);
            currentColor = new Color(fromColor.r, fromColor.g, fromColor.b, currentAlpha);
            _tileMap.color = currentColor;
            yield return null;
        }
    }
}
