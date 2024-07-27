using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _transform;
    [SerializeField] private List<Sprite> _spriteOptions;
    [SerializeField] private float _forceAddedOnSpawn = 10f;
    [SerializeField] private float _maxAngleDeviation = 30f;
    [SerializeField] private int _level;
    public int Level => _level;
    public Vector3 Position => _transform.position;
    public Rigidbody2D Rigidbody => _rigidbody;

    public void Setup(Vector3 position, Rigidbody2D copiedRigidbody = null)
    {
        _spriteRenderer.sprite = _spriteOptions[Random.Range(0, _spriteOptions.Count)];
        _transform.position = position;

        if (copiedRigidbody != null)
        {
            _rigidbody.position = copiedRigidbody.position;
            _rigidbody.rotation = copiedRigidbody.rotation;
            _rigidbody.velocity = copiedRigidbody.velocity;
            _rigidbody.angularVelocity = copiedRigidbody.angularVelocity;   
            AddForceOnSpawn(true);
        }
        else
        {
            AddForceOnSpawn(false);
        }
            
        
    }

    private void AddForceOnSpawn(bool addToCurrent)
    {
        // Get the current movement direction
        var currentDirection = _rigidbody.velocity.normalized;

        // Generate a random angle deviation within the specified range
        var randomAngle = Random.Range(-_maxAngleDeviation, _maxAngleDeviation);
        
        // Calculate the new direction by rotating the current direction by the random angle
        var newDirection = Quaternion.Euler(0, 0, randomAngle) * (addToCurrent ? currentDirection : Vector2.one);

        // Apply the force to the Rigidbody2D
        _rigidbody.AddForce(newDirection * _forceAddedOnSpawn, ForceMode2D.Impulse);
    }

    private void Update()
    {
        GameManager.Instance.KeepInBounds(_transform);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            GameManager.Instance.MetBullet(this, other.gameObject.GetComponent<Bullet>());
        }
        else if (other.gameObject.CompareTag("Spaceship"))
        {
            GameManager.Instance.CrashedIntoAsteroid(this);
        }
    }
}
