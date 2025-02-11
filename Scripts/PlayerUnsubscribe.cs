using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUnsubscribe : MonoBehaviour
{
    public event EventHandler OnUnsubscribeFromEvents;
    
    public void UnsubscribeFromEvents(){
        OnUnsubscribeFromEvents?.Invoke(this, EventArgs.Empty);
    }
}
