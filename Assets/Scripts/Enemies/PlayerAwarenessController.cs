using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool _awareOfPlayer {  get; private set; }
    public Vector2 _directionToPlayer {  get; private set; }

    [SerializeField]
    private float _PlayerAwarenessDistance;
    private Transform _player;
    void Start()
    {
        
    }

    private void Awake()
    {
        _player = Object.FindFirstObjectByType<PlayerController>().transform;
    }

    void Update()
    {
        Vector2 enemyToPlayerVector = _player.position - transform.position;
        _directionToPlayer = enemyToPlayerVector.normalized;

        if(enemyToPlayerVector.magnitude <= _PlayerAwarenessDistance)
        {
            _awareOfPlayer = true;
        }
        else
        {
            _awareOfPlayer = false;
        }
    }
}
