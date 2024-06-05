using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    public GameObject pauseMenu;
    public bool isPaused;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
        
        Time.timeScale = 1f;
    }

    private void Update()
    {
        // if (isPaused)
        // {
        //     ResumeGame();
        // }
        //
        // else
        // {
        //     PauseGame();
        // }
    }

    public void RestartScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

   public void LoadNextScene()
   {
       int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
       int nextSceneIndex = currentSceneIndex + 1;

       if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
       {
           nextSceneIndex = 0;
       }

       SceneManager.LoadScene(nextSceneIndex);

       if (nextSceneIndex > PlayerPrefs.GetInt("levelAt"))
       {
           PlayerPrefs.SetInt("levelAt", nextSceneIndex);
       }
   }

   public void CloseGame()
   {
       Application.Quit();
   }

   public void OpenScene(int sceneID)
   {
       SceneManager.LoadScene(sceneID);
   }

   public void PauseGame()
   {
       pauseMenu.SetActive(true);
       Time.timeScale = 0f;
       isPaused = true;
   }
   
   public void PauseGameWithoutMenu()
   {
       pauseMenu.transform.parent.gameObject.SetActive(false);
       Time.timeScale = 0f;
       isPaused = true;
   }

   public void ResumeGame()
   {
       pauseMenu.SetActive(false);
       Time.timeScale = 1f;
       isPaused = false;
   }
   
   
   
   
}
