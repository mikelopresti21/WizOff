using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItemHandler : MonoBehaviour
{
    private const string THROW = "Throw";
    private const string PICKUP = "Pickup";
    private const string USE_ITEM = "UseItem";
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerUnsubscribe playerUnsubscribe;
    [SerializeField] private PlayerMoveController playerMoveController;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Transform itemPosRight;
    [SerializeField] private Transform itemPosLeft;
    private Item nearbyItem;
    private bool isHoldingItem;
    private Item itemInhand;
    private bool isFacingRight;
    private float throwForce = 15f;

    void Start(){
        playerHealth = GetComponent<PlayerHealth>();

        Item.OnAnyItemDestroyed += Item_OnItemDestroyed;
        playerHealth.OnPlayerDied += PlayerHealth_OnPlayerDied;
        PlayersManager.Instance.OnAllPlayersReady += PlayersManager_OnAllPlayersReady;
        playerUnsubscribe.OnUnsubscribeFromEvents += PlayerUnsubscribe_OnUnsubscribeFromInputs;

    }

    void Update(){
        if (itemInhand != null){

            if (playerRB.linearVelocityX > 0 ){
                isFacingRight = true;
                if (itemInhand.transform.parent != itemPosRight){
                    SwitchHands(itemPosRight);
                }
            }
            else if (playerRB.linearVelocityX < 0){
                isFacingRight = false;
                if (itemInhand.transform.parent != itemPosLeft){
                    SwitchHands(itemPosLeft);
                }
            }
        }
    }

    private void Pickup_performed(InputAction.CallbackContext context){
        if (!GameManager.Instance.IsGamePlaying()){
            return;
        }

        if (!isHoldingItem && nearbyItem != null){
            PickupItem();
        } else if (isHoldingItem){
            DropItem();
        }
    }

    private void Throw_performed(InputAction.CallbackContext context){
        if (!GameManager.Instance.IsGamePlaying()){
            return;
        }

        if (isHoldingItem){
            ThrowItem(playerMoveController.GetMoveInput());
        }
    }

    private void UseItem_performed(InputAction.CallbackContext context){
        if (!GameManager.Instance.IsGamePlaying()){
            return;
        }
        
        if (isHoldingItem){
            itemInhand.UseItem();
        }
    }

    public void PickupItem(){
        isHoldingItem = true;
        itemInhand = nearbyItem;
        itemInhand.isHeld = true;
        itemInhand.itemRB.simulated = false;
        itemInhand.itemRB.bodyType = RigidbodyType2D.Kinematic;

        if (isFacingRight){
            itemInhand.transform.parent = itemPosRight;
        } else {
            itemInhand.transform.parent = itemPosLeft;
        }
        
        itemInhand.itemRB.linearVelocity = Vector2.zero;
        itemInhand.transform.localPosition = Vector3.zero;
        itemInhand.transform.localRotation = Quaternion.identity;
    }

    public void DropItem(){
        if (isHoldingItem){
            isHoldingItem = false;
            itemInhand.isHeld = false;
            itemInhand.itemRB.simulated = true;
            itemInhand.itemRB.bodyType = RigidbodyType2D.Dynamic;
            itemInhand.transform.parent = null;
            itemInhand = null;
        }
    }

    public void ThrowItem(Vector2 throwDirection){
        isHoldingItem = false;
        itemInhand.itemRB.simulated = true;
        itemInhand.itemRB.bodyType = RigidbodyType2D.Dynamic;
        itemInhand.transform.parent = null;
        
        itemInhand.GetComponent<Item>().ThrowItem();

        itemInhand.itemRB.linearVelocity = throwDirection * throwForce;
        itemInhand = null;
    }
    private void SwitchHands(Transform newParent){
        itemInhand.transform.parent = newParent;
        itemInhand.transform.localPosition = Vector3.zero;
        itemInhand.transform.localRotation = Quaternion.identity;
    }

    private void Item_OnItemDestroyed(object sender, EventArgs e){
        if (sender as Item == itemInhand){
            itemInhand = null;
            isHoldingItem = false;
        } else if (sender as Item == nearbyItem){
            nearbyItem = null;
        }
    }

    private void UnsubscribeFromEvents(){
        Item.OnAnyItemDestroyed -= Item_OnItemDestroyed;
        playerHealth.OnPlayerDied -= PlayerHealth_OnPlayerDied;
        PlayersManager.Instance.OnAllPlayersReady -= PlayersManager_OnAllPlayersReady;
        playerUnsubscribe.OnUnsubscribeFromEvents -= PlayerUnsubscribe_OnUnsubscribeFromInputs;
    }

    private void PlayerUnsubscribe_OnUnsubscribeFromInputs(object sender, EventArgs e){
        DisableControls();
        UnsubscribeFromEvents();
    }

    private void PlayerHealth_OnPlayerDied(object sender, EventArgs e){
        if (itemInhand != null){
            DropItem();
        } 
    }

    private void PlayersManager_OnAllPlayersReady(object sender, EventArgs e){
        EnableControls();
    }

    private void EnableControls(){
        playerInput.actions[THROW].performed += Throw_performed;
        playerInput.actions[PICKUP].performed += Pickup_performed;
        playerInput.actions[USE_ITEM].performed += UseItem_performed;
    }

    private void DisableControls(){
        playerInput.actions[THROW].performed -= Throw_performed;
        playerInput.actions[PICKUP].performed -= Pickup_performed;
        playerInput.actions[USE_ITEM].performed -= UseItem_performed;
    }

    void OnTriggerEnter2D(Collider2D trigger){
        if (trigger.GetComponent<Item>() != null) {
            if (trigger.GetComponent<Item>().CanPickup()) {
                nearbyItem = trigger.GetComponent<Item>();
            }
        }
    }

    void OnTriggerExit2D(Collider2D trigger){
        if (trigger.gameObject.GetComponent<Item>() == nearbyItem){
            nearbyItem = null;
        }
    }
}
