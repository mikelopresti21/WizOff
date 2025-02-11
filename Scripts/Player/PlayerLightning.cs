using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLightning : MonoBehaviour
{
    private const string LIGHTNING = "Lightning";
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerUnsubscribe playerUnsubscribe;
    [SerializeField] private Transform right;
    [SerializeField] private Transform left;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private GameObject lightningPrefab;
    private Transform lightningPos;
    private Lightning lightning;
    private bool flipped;
    
    private void Start(){

        PlayersManager.Instance.OnAllPlayersReady += PlayersManager_OnAllPlayersReady;
        playerUnsubscribe.OnUnsubscribeFromEvents += PlayerUnsubscribe_OnUnsubscribeFromInputs;
    }

    void Update(){

        if (lightning == null){
            flipped = false;
        }

        if (playerRB.linearVelocityX > 0){
            lightningPos = right;
            if (lightning != null && flipped){
                FlipHorizontally();     
                lightning.transform.position = lightningPos.position;     
            }  
        } else if (playerRB.linearVelocityX < 0){
            lightningPos = left;
            if (lightning != null && !flipped){
                FlipHorizontally();
                lightning.transform.position = lightningPos.position; 
            }
        }    
    }

    public void RemoveLightning(){
        if (lightning != null){
            lightning.DestroyLightning();
            lightning = null;
        }
    }

    private void Lightning_started(InputAction.CallbackContext context){
        if (!GameManager.Instance.IsGamePlaying()){
            return;
        }
        
        if (lightning == null && lightningPos != null){
            lightning = Instantiate(lightningPrefab, lightningPos.position, Quaternion.identity).GetComponent<Lightning>();
            lightning.transform.parent = lightningPos;
        }
    }

    private void Lightning_canceled(InputAction.CallbackContext context){
        if (!GameManager.Instance.IsGamePlaying()){
            return;
        }
        
        RemoveLightning();
    }

    public void FlipHorizontally(){
        if (lightning != null)
        {
            Vector3 scale = lightning.transform.localScale;
            scale.x = -scale.x;
            lightning.transform.localScale = scale;
            flipped = !flipped;
        }
    }

    private void UnsubscribeFromEvents(){
        PlayersManager.Instance.OnAllPlayersReady -= PlayersManager_OnAllPlayersReady;
        playerUnsubscribe.OnUnsubscribeFromEvents -= PlayerUnsubscribe_OnUnsubscribeFromInputs;
    }

    private void PlayerUnsubscribe_OnUnsubscribeFromInputs(object sender, EventArgs e){
        DisableControls();
        UnsubscribeFromEvents();
    }

    private void PlayersManager_OnAllPlayersReady(object sender, EventArgs e){
        EnableControls();
    }

    private void EnableControls(){
        playerInput.actions[LIGHTNING].started += Lightning_started;
        playerInput.actions[LIGHTNING].canceled += Lightning_canceled;
    }

    private void DisableControls(){
        playerInput.actions[LIGHTNING].started += Lightning_started;
        playerInput.actions[LIGHTNING].canceled += Lightning_canceled;
    }
}
