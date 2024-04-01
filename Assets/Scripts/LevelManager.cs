using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform playerSpawn;

    public GameObject pauseMenu;
    public GameObject victoryMenu;
    public GameObject defeatMenu;

    private void Start()
    {
        Time.timeScale = 1;
        StartGame();
    }

    public void ReStart()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        GameObject go = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        FindObjectOfType<MyCamera>().myTarget = go.transform;
    }

    public void PlayerDeath()
    {
        defeatMenu.SetActive(true);
    }

    public void Victory()
    {
        Time.timeScale = 0;
        victoryMenu.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
        EditorApplication.isPlaying = false;
    }
}
