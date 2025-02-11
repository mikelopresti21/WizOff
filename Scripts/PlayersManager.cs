using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayersManager : MonoBehaviour
{
    private const string PLAYER_ACTION_MAP = "Player";
    public static PlayersManager Instance { get; private set; }
    public class Winner {
        public Sprite winnerSprite;
        public int playerIndex;
    }
    public event EventHandler OnAllPlayersReady;
    public event EventHandler OnPlayerReset;
    [SerializeField] private PlayerInput player1;
    [SerializeField] private PlayerInput player2;
    [SerializeField] private Transform mainLayOut; 
    [SerializeField] private GameObject playerSelectorPrefab;
    private Winner winner;
    

    public void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(this);   
        }
    }

    public void HandlerPlayerJoin(PlayerInput playerInput){
        if (!GameManager.Instance.IsInCharacterSelection()){
            return;
        }

        if (player1 != null && player2 != null){
            return;
        }

        playerInput.transform.SetParent(transform);

        if (player1 == null){
            player1 = playerInput;
            SetUpPlayerSelection(player1);
        } else if (player2 == null){
            player2 = playerInput;
            SetUpPlayerSelection(player2);
        }
    }

    private void SetUpPlayerSelection(PlayerInput playerInput){
        PlayerSelector newPlayerSelector = Instantiate(playerSelectorPrefab, mainLayOut).GetComponent<PlayerSelector>();
        newPlayerSelector.SetPlayerIndex(playerInput.playerIndex);
        newPlayerSelector.SetTitle();
        playerInput.GetComponent<PlayerSelectionController>().SetPlayerSelector(newPlayerSelector);
    }

    public void CheckAllPlayersReady(){
        if (player1 == null || player2 == null){
            return;
        }

        if (player1.GetComponent<PlayerSelectionController>().isReady && player2.GetComponent<PlayerSelectionController>().isReady){
            player1.SwitchCurrentActionMap(PLAYER_ACTION_MAP);
            player2.SwitchCurrentActionMap(PLAYER_ACTION_MAP);
            OnAllPlayersReady?.Invoke(this, EventArgs.Empty);
            SceneManager.LoadScene(Loader.Scene.StartStage.ToString());
        }
    }

    public PlayerInput GetPlayer1Input(){
        return player1.GetComponent<PlayerInput>();
    }

    public PlayerInput GetPlayer2Input(){
        return player2.GetComponent<PlayerInput>();
    }

    public void SetWinner(PlayerInput winner){
        this.winner = new Winner();
        this.winner.winnerSprite = winner.GetComponentInChildren<PlayerVisual>().GetPlayerSprite();
        this.winner.playerIndex = winner.playerIndex;
    }

    public Sprite GetWinnerSprite(){
        return winner.winnerSprite;
    }

    public int GetWinnerIndex(){
        return winner.playerIndex;
    }

    public void DestroyPlayers(){
        player1.GetComponent<PlayerUnsubscribe>().UnsubscribeFromEvents();
        player2.GetComponent<PlayerUnsubscribe>().UnsubscribeFromEvents();
        Destroy(player1.gameObject);
        Destroy(player2.gameObject);
        player1 = null;
        player2 = null;
    }

    public void DestroySelf(){
        DestroyPlayers();
        Instance = null;
        Destroy(gameObject);
    }

    public void DisplayPlayers(){
        player1.GetComponentInChildren<PlayerVisual>(true).ShowPlayerVisual();
        player2.GetComponentInChildren<PlayerVisual>(true).ShowPlayerVisual();
    }

    public void ResetPlayers(){
        OnPlayerReset.Invoke(this, EventArgs.Empty);
    }
}
