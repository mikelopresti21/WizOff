using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour
{
    private const string MOVE = "Move";
    private const string JUMP = "Jump";
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerUnsubscribe playerUnsubscribe;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private PlayerCollisions playerCollisions;
    [SerializeField] private LayerMask groundLayerMask;
    private Vector2 moveInput;
    private float moveSpeed;
    private float slowMoveSpeed = 4f;
    private float normalMoveSpeed = 8f;
    private float jumpForce = 10f;
    private int jumpCount;
    private bool isGrounded;
    private bool controlsAreReversed;
    private bool isEnabled;
    
    private void Start(){

        moveSpeed = normalMoveSpeed;
        
        playerCollisions.OnSpellCollided += PlayerSpells_OnSpellCollided;
        PlayersManager.Instance.OnAllPlayersReady += PlayersManager_OnAllPlayersReady;
        playerUnsubscribe.OnUnsubscribeFromEvents += PlayerUnsubscribe_OnUnsubscribeFromInputs;
        PlayersManager.Instance.OnPlayerReset += PlayersManager_OnPlayerReset;
    }

    void Update(){
        if (playerRB != null){
            MovePlayer();
        }

        if (isGrounded == true){
            jumpCount = 0;
        }
    }

    public void MovePlayer(){
        if (!GameManager.Instance.IsGamePlaying()){
            return;
        }
        
        if (isEnabled){
            moveInput = playerInput.actions[MOVE].ReadValue<Vector2>();
            if (controlsAreReversed){
                playerRB.linearVelocity = new Vector2(-moveInput.x * moveSpeed, playerRB.linearVelocityY);
            } else {
                playerRB.linearVelocity = new Vector2(moveInput.x * moveSpeed, playerRB.linearVelocityY);
            }
        }
    }

    private void Jump_performed(InputAction.CallbackContext context){
        if (!GameManager.Instance.IsGamePlaying()){
            return;
        }

        if (jumpCount < 1){
            playerRB.linearVelocityY = 0;
            playerRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount ++;
        }
    }

    public void SlowSpeed(){
        moveSpeed = slowMoveSpeed;
        StartCoroutine(ResetSpeed());
    }

    IEnumerator ResetSpeed(){
        yield return new WaitForSeconds(3f);
        moveSpeed = normalMoveSpeed;
    }

    public void ReverseControls(){
        controlsAreReversed = true;
        StartCoroutine(ResetReverse());
    }

    IEnumerator ResetReverse(){
        yield return new WaitForSeconds(3f);
        controlsAreReversed = false;
    }

    public Vector2 GetMoveInput(){
        return playerInput.actions[MOVE].ReadValue<Vector2>();
    }

    private void EnableControls(){
        playerInput.actions[JUMP].performed += Jump_performed;
        isEnabled = true;
    }

    private void DisableControls(){
        playerInput.actions[JUMP].performed -= Jump_performed;
        isEnabled = false;
    }

    private void UnsubscribeFromEvents(){
        playerCollisions.OnSpellCollided -= PlayerSpells_OnSpellCollided;
        PlayersManager.Instance.OnAllPlayersReady -= PlayersManager_OnAllPlayersReady;
        playerUnsubscribe.OnUnsubscribeFromEvents -= PlayerUnsubscribe_OnUnsubscribeFromInputs;
    }

    private void ResetMovement(){
        moveSpeed = normalMoveSpeed;
        controlsAreReversed = false;
    }

    private void PlayersManager_OnPlayerReset(object sender, EventArgs e){
        ResetMovement();
    }

    private void PlayerUnsubscribe_OnUnsubscribeFromInputs(object sender, EventArgs e){
        DisableControls();
        UnsubscribeFromEvents();
    }

    private void PlayersManager_OnAllPlayersReady(object sender, EventArgs e){
        EnableControls();
    }

    private void PlayerSpells_OnSpellCollided(object sender, PlayerCollisions.OnSpellCollidedEventArgs e){
        switch (e.spellType){
            case Spell.SpellType.Reverse:
                ReverseControls();
                break;
            case Spell.SpellType.Slow:
                SlowSpeed();
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision){
        if (((1 << collision.gameObject.layer) & groundLayerMask) != 0){
            isGrounded = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision){
        isGrounded = ((1 << collision.gameObject.layer) & groundLayerMask) != 0;
    }
}