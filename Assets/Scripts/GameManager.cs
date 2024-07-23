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
    
    public bool IsAlive => _isAlive;
    
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
    
    private void Awake()
    {
        SingletonUpkeep();
    }
    
}