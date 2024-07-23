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
    public bool IsAlive => _isAlive;
    
    private void Awake()
    {
        SingletonUpkeep();
    }
    
}