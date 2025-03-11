using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    void Start()
    {
        menuCanvas.SetActive(false);
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            Time.timeScale = menuCanvas.activeSelf ? 1f : 0f;
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
    }
}
