using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnScreenButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private Action _onPressTrue;
    private Action _onPressFalse;
    private bool _pressed;

    public void Setup(Action onPressTrue, Action onPressFalse)
    {
        _onPressTrue = onPressTrue;
        _onPressFalse = onPressFalse;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _pressed = true;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        _pressed = false;
    }
    
    
    private void Update()
    {
        if (_pressed)
        {
            _onPressTrue?.Invoke();
        }
        else
        {
            _onPressFalse?.Invoke();
        }
    }
}