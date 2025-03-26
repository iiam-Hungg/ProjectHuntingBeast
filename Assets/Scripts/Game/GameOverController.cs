using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    private void Start()
    {
        
    }
    public void RestartGame()
    {
        Debug.Log("Restart button clicked!");
        Time.timeScale = 1;
        Destroy(PlayerHealth.Instance.gameObject);
        SceneManager.LoadScene("Scene1");
    }

    public void BackToMainMenu()
    {
        Debug.Log("Main Title button clicked!");
        SceneManager.LoadScene("Title");
    }
}
