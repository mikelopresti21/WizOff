using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerFinish : MonoBehaviour
{
    [SerializeField] private int playerKey;

    public void OnTriggerEnter2D(Collider2D trigger){
        PlayerInput playerInput = trigger.GetComponent<PlayerInput>();

        if (playerInput != null && playerInput.playerIndex == playerKey){
            PlayersManager.Instance.SetWinner(playerInput);
            Loader.LoadWinScene();
        }
    }
}
