using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    #region Singleton

    private static InputController _instance;
    
    public static InputController Instance
    {
        get
        {
            if (_instance != null) return _instance;

            // detect in scene
            _instance = FindAnyObjectByType<InputController>();

            if (_instance != null) return _instance;
            
            // create new
            var singletonObject = new GameObject(typeof(InputController).ToString());
            _instance = singletonObject.AddComponent<InputController>();
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

    private void Awake()
    {
        SingletonUpkeep();
    }

    private void Update()
    {
        RotatingLeft = Input.GetKey(KeyCode.LeftArrow);
        RotatingRight = Input.GetKey(KeyCode.RightArrow);
        PressingFire = Input.GetKey(KeyCode.RightArrow);
        PressingThrottle = Input.GetKey(KeyCode.RightArrow);
    }

    public bool RotatingLeft { get; set; }

    public bool RotatingRight { get; set; }

    public bool PressingThrottle { get; set; }

    public bool PressingFire { get; set; }
}