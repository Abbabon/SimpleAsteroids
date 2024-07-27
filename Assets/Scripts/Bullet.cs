using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    private Transform _transform;

    public Rigidbody2D Rigidbody => _rigidbody;
    public bool TimedOut { get; set; }

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        GameManager.Instance.KeepInBounds(_transform);
    }
}