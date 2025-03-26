using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Loading Scene: Scene1");
        Time.timeScale = 1;
        if (PlayerHealth.Instance != null)
        {
            Destroy(PlayerHealth.Instance.gameObject);
        }
        else
        {
            Debug.LogWarning("PlayerHealth instance is already null or does not exist in this scene.");
        }
        SceneManager.LoadScene("Scene1");
    }

    public void ExitGame()
    {
        Application.Quit(); 
      #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
        #endif
    }
}
