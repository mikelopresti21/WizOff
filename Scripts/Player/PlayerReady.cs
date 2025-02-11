using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReady : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    public bool isReady;
    
    public void InitializePlayer(){
        
        playerInput.SwitchCurrentActionMap("Player");
    }

    public override string ToString(){
        return "Player " + (GetComponent<PlayerInput>().playerIndex + 1);
    }
}
