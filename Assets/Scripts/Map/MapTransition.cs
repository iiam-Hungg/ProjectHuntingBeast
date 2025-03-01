using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MapTransition : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D mapBoundry;
    private CinemachineConfiner confiner;

    [SerializeField] private Direction direction;
    [SerializeField] private float transitionTime = 0.5f;
    [SerializeField] private Image fadeScreen;

    private Collider2D triggerCollider;
    private bool isTransitioning = false;

    [SerializeField] private MapTransition oppositeTrigger;

    private enum Direction { Up, Down, Left, Right }

    private void Awake()
    {
        confiner = FindFirstObjectByType<CinemachineConfiner>();
        triggerCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTransitioning)
        {
            isTransitioning = true;
            triggerCollider.enabled = false;

            if (oppositeTrigger != null)
            {
                oppositeTrigger.DisableTrigger();
            }

            StartCoroutine(TransitionPlayer(collision.gameObject));
        }
    }


    public void DisableTrigger()
    {
        triggerCollider.enabled = false;
    }

    private IEnumerator TransitionPlayer(GameObject player)
    {
        if (fadeScreen != null) yield return StartCoroutine(FadeScreen(1));

        Collider2D playerCollider = player.GetComponent<Collider2D>();
        if (playerCollider != null) playerCollider.enabled = false;

        Vector3 startPos = player.transform.position;
        Vector3 targetPos = GetTargetPosition(startPos);

        float elapsedTime = 0;
        while (elapsedTime < transitionTime)
        {
            player.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / transitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        player.transform.position = targetPos;

        confiner.m_BoundingShape2D = mapBoundry;

        if (fadeScreen != null) yield return StartCoroutine(FadeScreen(0));

        yield return new WaitForSeconds(0.5f);

        if (playerCollider != null) playerCollider.enabled = true;

        isTransitioning = false;

        yield return new WaitForSeconds(0.5f);

        triggerCollider.enabled = true;

        if (oppositeTrigger != null)
        {
            oppositeTrigger.EnableTrigger();
        }
    }


    public void EnableTrigger()
    {
        triggerCollider.enabled = true;
    }

    private Vector3 GetTargetPosition(Vector3 currentPos)
    {
        float moveDistance = 1.0f;
        switch (direction)
        {
            case Direction.Up: return currentPos + Vector3.up * moveDistance;
            case Direction.Down: return currentPos + Vector3.down * moveDistance;
            case Direction.Left: return currentPos + Vector3.left * moveDistance;
            case Direction.Right: return currentPos + Vector3.right * moveDistance;
            default: return currentPos;
        }
    }

    private IEnumerator FadeScreen(float targetAlpha)
    {
        float fadeDuration = 0.5f;
        float startAlpha = fadeScreen.color.a;
        float time = 0;

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            fadeScreen.color = new Color(0, 0, 0, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        fadeScreen.color = new Color(0, 0, 0, targetAlpha);
    }
}

