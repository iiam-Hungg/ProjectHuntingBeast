using Cinemachine;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip t1Music;
    public AudioClip f1Music;

    public PolygonCollider2D t1Boundary;
    public PolygonCollider2D f1Boundary;

    private string currentMap;

    void Start()
    {
        UpdateMusic();
    }

    void Update()
    {
        UpdateMusic();
    }

    private void UpdateMusic()
    {
        if (t1Boundary == null || f1Boundary == null)
        {
            Debug.LogWarning("T1 or F1 PolygonCollider2D is not assigned!");
            return;
        }


        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player not found!");
            return;
        }

        Vector2 playerPosition = player.transform.position;


        string newMap = currentMap;
        if (IsInsidePolygon(t1Boundary, playerPosition))
        {
            newMap = "T1";
        }
        else if (IsInsidePolygon(f1Boundary, playerPosition))
        {
            newMap = "F1";
        }

        if (newMap != currentMap)
        {
            currentMap = newMap;
            if (currentMap == "T1")
                audioSource.clip = t1Music;
            else if (currentMap == "F1")
                audioSource.clip = f1Music;

            if (audioSource.clip != null)
                audioSource.Play();
        }
    }

    private bool IsInsidePolygon(PolygonCollider2D polygon, Vector2 point)
    {
        return polygon.OverlapPoint(point);
    }
}
