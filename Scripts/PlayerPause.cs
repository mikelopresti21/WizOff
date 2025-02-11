using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPause : MonoBehaviour
{
    private const string PUASE = "Pause";
    public static event EventHandler OnAnyPlayerPause;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerUnsubscribe playerInputUnsubscribe;
    


    private void Start(){
        PlayersManager.Instance.OnAllPlayersReady += PlayersManager_OnAllPlayersReady;
        playerInputUnsubscribe.OnUnsubscribeFromEvents += PlayerInputUnsubscribe_OnUnsubscribeFromInputs;
    }

    private void EnableControls(){
        playerInput.actions[PUASE].performed += Pause_performed;
    }

    private void DisableControls(){
        playerInput.actions[PUASE].performed -= Pause_performed;
    }

    private void Pause_performed(InputAction.CallbackContext context){
        OnAnyPlayerPause?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerInputUnsubscribe_OnUnsubscribeFromInputs(object sender, EventArgs e){
        DisableControls();
    }

    private void PlayersManager_OnAllPlayersReady(object sender, EventArgs e){
        EnableControls();
    }

}
