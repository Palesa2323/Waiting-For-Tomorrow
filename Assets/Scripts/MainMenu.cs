using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Load the game scene, assuming it's named "GameScene"
        SceneManager.LoadScene("GameScene");
    }

    public void SaveGame()
    {
        // Implement load game logic here (if you have saved data)
        Debug.Log("Load Game clicked");
    }

    public void OpenSettings()
    {
        // Open settings menu or scene
        Debug.Log("Settings clicked");
    }

    public void QuitGame()
    {
        // Quit the application
        Debug.Log("Quit Game");
        Application.Quit();

        // If running in the editor, stop playing the scene
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
