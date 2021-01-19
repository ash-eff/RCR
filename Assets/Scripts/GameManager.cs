using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winGameScreen;
    [SerializeField] private GameObject pauseScreen;
    public int numberOfRoomsToUnlock;
    private int numberOfRoomsUnlocked;
    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
    }

    private void PauseMenu()
    {
        isPaused = !isPaused;
        Cursor.visible = isPaused;
        pauseScreen.SetActive(isPaused);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void RoomUnlocked()
    {
        numberOfRoomsUnlocked++;
        if (numberOfRoomsUnlocked >= numberOfRoomsToUnlock)
        {
            Cursor.visible = true;
            winGameScreen.SetActive(true);
        }
    }
}
