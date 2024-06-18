using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance; // Static instance for singleton pattern
    public GameObject pauseMenu; // Reference to the pause menu UI
    public bool isPaused; // Flag to check if the game is paused

    private void Awake() // Awake is called when the script instance is being loaded
    {
        instance = this;
    }
    private void Start()  // Start is called before the first frame update
    {
        if (pauseMenu != null)
        {
            // Ensure the pause menu is initially hidden
            pauseMenu.SetActive(false);
        }
        
        Time.timeScale = 1f; // Set the time scale to 1 (normal speed)
    }
    public void RestartScene() // Method to restart the current scene
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
   public void LoadNextScene() // Method to load the next scene in the build settings
   {
       int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
       int nextSceneIndex = currentSceneIndex + 1;

       if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
       {
           nextSceneIndex = 0;
       }

       SceneManager.LoadScene(nextSceneIndex);

       // Update PlayerPrefs with the highest unlocked level
       if (nextSceneIndex > PlayerPrefs.GetInt("levelAt"))
       {
           PlayerPrefs.SetInt("levelAt", nextSceneIndex);
       }
   }
   public void CloseGame() // Method to close the game
   {
       Application.Quit();
   }
   public void OpenScene(int sceneID) // Method to load a specific scene by its ID
   {
       SceneManager.LoadScene(sceneID);
   }
   public void PauseGame() // Method to pause the game and show the pause menu
   {
       pauseMenu.SetActive(true);
       Time.timeScale = 0f;
       isPaused = true;
   }
   public void PauseGameWithoutMenu() // Method to pause the game without showing the pause menu
   {
       pauseMenu.transform.parent.gameObject.SetActive(false);
       Time.timeScale = 0f;
       isPaused = true;
   }
   public void ResumeGame() // Method to resume the game from the paused state
   {
       pauseMenu.SetActive(false);
       Time.timeScale = 1f;
       isPaused = false;
   }
}
