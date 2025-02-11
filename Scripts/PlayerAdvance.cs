using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PlayerAdvance : MonoBehaviour
{
    [SerializeField] private int playerKey;

    public void OnTriggerEnter2D(Collider2D trigger){
        PlayerInput playerInput = trigger.GetComponent<PlayerInput>();

        if (playerInput != null && playerInput.playerIndex == playerKey){
            if (ItemSpawner.Instance.GetItemInPlay() != null){
                Item item = ItemSpawner.Instance.GetItemInPlay();
                if (!item.isHeld){
                    item.DestroySelf();
                }
            }
            if (playerInput.playerIndex == 0){
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            } else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }
    }
}
