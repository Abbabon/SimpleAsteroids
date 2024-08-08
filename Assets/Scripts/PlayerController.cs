using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Transform _transform;
    [SerializeField] private Animator _animator;

    [Header("Parameters")]
    [SerializeField] private float _shipRotationSpeed = 200f;
    [SerializeField] private float _thrustForce = 50f;
    [SerializeField] private float _projectileSpeed = 10f;
    [SerializeField] private float _cooldownBetweenShots = 10f;
    [SerializeField] private float _bulletTimeout = 0.8f;

    private bool _onCooldown;
    private PrefabObjectPool<Bullet> _prefabObjectPool;
    private static readonly int IsFlying = Animator.StringToHash("IsFlying");

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
        var pressingThrottle = InputController.Instance.PressingThrottle;
        _animator.SetBool(IsFlying, pressingThrottle);
        
        if (!pressingThrottle) return;
        var thrustDirection = _transform.up;
        _rigidbody2D.AddForce(thrustDirection * _thrustForce * Time.deltaTime);
    }
    
    private void HandleBullets()
    {
        if (!InputController.Instance.PressingFire)
            return;

        if (_onCooldown)
            return;

        StartCoroutine(ShootCooldown());
        
        var bullet = _prefabObjectPool.GetObject();
        bullet.transform.position = _transform.position;
        bullet.Rigidbody.velocity = _transform.up * _projectileSpeed;

        bullet.TimedOut = false;
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
        var startTime = Time.time;

        while (Time.time - startTime < _bulletTimeout && !bullet.TimedOut)
        {
            yield return null;
        }
        _prefabObjectPool.ReturnObject(bullet);
    } 
    
    
    private void HandleRotation()
    {
        if (InputController.Instance.RotatingLeft)
        {
            _transform.Rotate(_shipRotationSpeed * Time.deltaTime * transform.forward);
        }
        else if (InputController.Instance.RotatingRight)
        {
            _transform.Rotate(_shipRotationSpeed * Time.deltaTime * transform.forward * -1);
        }
    }

    public void ReturnBullet(Bullet bullet)
    {
        bullet.Rigidbody.HaltRigidbody();
        bullet.TimedOut = true;
    }
}
