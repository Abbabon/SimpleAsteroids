using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    // TODO: activate only on mobile

    [SerializeField] private OnScreenButton _leftButton;
    [SerializeField] private OnScreenButton _rightButton;
    [SerializeField] private OnScreenButton _aButton;
    [SerializeField] private OnScreenButton _bButton;
    
    private void Awake()
    {
        _leftButton.Setup(() => InputController.Instance.RotatingLeft = true,
            () => InputController.Instance.RotatingLeft = false);
        
        _rightButton.Setup(() => InputController.Instance.RotatingRight = true,
            () => InputController.Instance.RotatingRight = false);
        
        _aButton.Setup(() => InputController.Instance.PressingThrottle = true,
            () => InputController.Instance.PressingThrottle = false);
        
        _bButton.Setup(() => InputController.Instance.PressingFire = true,
            () => InputController.Instance.PressingFire = false);
    }

}
