using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Transform _transform;

    [Header("Parameters")]
    [SerializeField] private float _shipRotationSpeed = 200f;
    [SerializeField] private float _thrustForce = 50f;
    [SerializeField] private float _projectileSpeed = 10f;
    [SerializeField] private float _cooldownBetweenShots = 10f;
    [SerializeField] private float _bulletTimeout = 0.8f;

    private bool _onCooldown;
    private PrefabObjectPool<Bullet> _prefabObjectPool;
    
    private void Awake()
    {
        _prefabObjectPool = new PrefabObjectPool<Bullet>(_bulletPrefab);
    }

    private void Update()
    {
        if (!GameManager.Instance.IsAlive) return;

        GameManager.Instance.KeepInBounds(_transform);
        HandleAcceleration();
        HandleRotation();
        HandleBullets();
    }

    private void HandleAcceleration()
    {
        if (!Input.GetKey(KeyCode.UpArrow)) return;
        
        var thrustDirection = _transform.up;
        _rigidbody2D.AddForce(thrustDirection * _thrustForce * Time.deltaTime);
    }
    
    private void HandleBullets()
    {
        if (!Input.GetKey(KeyCode.Space))
            return;

        if (_onCooldown)
            return;

        StartCoroutine(ShootCooldown());
        
        var bullet = _prefabObjectPool.GetObject();
        bullet.transform.position = _transform.position;
        bullet.Rigidbody.velocity = _transform.up * _projectileSpeed;
        StartCoroutine(SelfDestruct(bullet));
    }

    
    private IEnumerator ShootCooldown()
    {
        _onCooldown = true;
        yield return new WaitForSeconds(_cooldownBetweenShots);
        _onCooldown = false;
    }
    
    private IEnumerator SelfDestruct(Bullet bullet)
    {
        yield return new WaitForSeconds(_bulletTimeout);
        _prefabObjectPool.ReturnObject(bullet);
    }
    
    private static bool RotatingLeft => Input.GetKey(KeyCode.LeftArrow); 
    private static bool RotatingRight => Input.GetKey(KeyCode.RightArrow); 
    
    
    private void HandleRotation()
    {
        if (RotatingLeft)
        {
            _transform.Rotate(_shipRotationSpeed * Time.deltaTime * transform.forward);
        }
        else if (RotatingRight)
        {
            _transform.Rotate(_shipRotationSpeed * Time.deltaTime * transform.forward * -1);
        }
    }
}
