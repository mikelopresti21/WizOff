using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSelectionController : MonoBehaviour
{   
    private const string NEXT_CHARACTER = "NextCharacter";
    private const string PREVIOUS_CHARACTER = "PreviousCharacter";
    private const string READY = "Ready";
    private const string CANCEL_READY = "CancelReady";
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private PlayerInput playerInput;
    private PlayerSelector playerSelector;
    public bool isReady;
    
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        EnableControls();
        PlayersManager.Instance.OnAllPlayersReady += PlayersManager_OnAllPlayersReady;
    }

    public void EnableControls(){
        playerInput.actions[NEXT_CHARACTER].performed += NextCharcter_performed;
        playerInput.actions[PREVIOUS_CHARACTER].performed += PreviousCharcter_performed;
        playerInput.actions[READY].performed += Ready_performed;
        playerInput.actions[CANCEL_READY].performed += CancelReady_performed;
    }

    public void DisabeControls(){
        playerInput.actions[NEXT_CHARACTER].performed -= NextCharcter_performed;
        playerInput.actions[PREVIOUS_CHARACTER].performed -= PreviousCharcter_performed;
        playerInput.actions[READY].performed -= Ready_performed;
        playerInput.actions[CANCEL_READY].performed -= CancelReady_performed;
    }

    private void Ready_performed(InputAction.CallbackContext context){
        if (!isReady){
            isReady = true;
            playerVisual.SetPlayerSprite(playerSelector.GetCurrentSprite());
            playerVisual.SetRuntimeAnimatorController(playerSelector.GetCurrentAnimator());
            playerSelector.ShowReady();
            PlayersManager.Instance.CheckAllPlayersReady();
        }
    }

    private void CancelReady_performed(InputAction.CallbackContext context){
        isReady = false;
        playerSelector.HideReady();
    }

    private void NextCharcter_performed(InputAction.CallbackContext context){
        playerSelector.NextCharacter(); 
    }

    private void PreviousCharcter_performed(InputAction.CallbackContext context){
        playerSelector.PrevCharacter();
    } 

    public void SetPlayerSelector(PlayerSelector playerSelector){
        this.playerSelector = playerSelector;
    }

    public PlayerSelector GetPlayerSelector(){
        return playerSelector;
    }    

    private void PlayersManager_OnAllPlayersReady(object sender, EventArgs e){
        DisabeControls();
        PlayersManager.Instance.OnAllPlayersReady -= PlayersManager_OnAllPlayersReady;
    }
}
