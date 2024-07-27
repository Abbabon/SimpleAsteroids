using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;
    
    public static GameManager Instance
    {
        get
        {
            if (_instance != null) return _instance;

            // detect in scene
            _instance = FindAnyObjectByType<GameManager>();

            if (_instance != null) return _instance;
            
            // create new
            var singletonObject = new GameObject(typeof(GameManager).ToString());
            _instance = singletonObject.AddComponent<GameManager>();
            return _instance;
        }
    }
    
    private void SingletonUpkeep()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
 
    // TODO: game loop
    [SerializeField] private bool _isAlive = true;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private PlayerController _playerController;
    
    public bool IsAlive => _isAlive;

    [SerializeField] private AsteroidData _asteroidData;
    private Dictionary<int, PrefabObjectPool<Asteroid>> _astroidPools;

    [SerializeField] private List<GameObject> _initialPositions;
    
    private void Awake()
    {
        SingletonUpkeep();

        _astroidPools = new Dictionary<int, PrefabObjectPool<Asteroid>>();
        foreach (var asteroidLevel in _asteroidData.AsteroidLevels)
        {
            _astroidPools[asteroidLevel.Level] = new PrefabObjectPool<Asteroid>(asteroidLevel.Prefab);
        }

        StartGame();
    }

    private void StartGame()
    {
        foreach (var initialPosition in _initialPositions)
        {
            SpawnAsteroid(1, initialPosition.transform.position);
        }
    }

    public void KeepInBounds(Transform checkedTransform)
    {
        var viewportPosition = _mainCamera.WorldToViewportPoint(checkedTransform.position);
        var outOfBounds = false;
        
        switch (viewportPosition.x)
        {
            case < 0:
                viewportPosition.x = 1;
                outOfBounds = true;
                break;
            case > 1:
                viewportPosition.x = 0;
                outOfBounds = true;
                break;
        }

        switch (viewportPosition.y)
        {
            case < 0:
                viewportPosition.y = 1;
                outOfBounds = true;
                break;
            case > 1:
                viewportPosition.y = 0;
                outOfBounds = true;
                break;
        }

        if (!outOfBounds) return;
        
        var newWorldPosition = _mainCamera.ViewportToWorldPoint(viewportPosition);
        checkedTransform.position = newWorldPosition;
    }

    public void MetBullet(Asteroid asteroid, Bullet bullet)
    {
        //TODO: explosion
        SpawnSubAsteroids(asteroid);

        var asteroidPool = _astroidPools[asteroid.Level];
        asteroid.Rigidbody.HaltRigidbody();
        asteroidPool.ReturnObject(asteroid);
        
        _playerController.ReturnBullet(bullet);
    }

    private void SpawnSubAsteroids(Asteroid destroyedAsteroid)
    {
        var asteroidsShouldSpawn = destroyedAsteroid.Level < _astroidPools.Count;
        if (!asteroidsShouldSpawn) return;
        
        var level = destroyedAsteroid.Level + 1;
        SpawnAsteroid(level, destroyedAsteroid.Position, destroyedAsteroid.Rigidbody);
        SpawnAsteroid(level, destroyedAsteroid.Position, destroyedAsteroid.Rigidbody);
    }

    private void SpawnAsteroid(int level, Vector3 position, Rigidbody2D rigidbody = null)
    {
        var newAsteroid = _astroidPools[level].GetObject();
        newAsteroid.Setup(position, rigidbody);
    }

    public void CrashedIntoAsteroid(Asteroid asteroid)
    {
        
    }
}