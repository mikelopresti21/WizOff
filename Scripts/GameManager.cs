using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event EventHandler OnGameOver;
    public event EventHandler OnCountDownStarted;
    private enum State {
        CharacterSelection,
        CountDown,
        Playing,
        Paused,
        GameOver,
    }
    private State state;
    private float countDownTimer;
    private float countDownTimerMax = 3f;

    private void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(this);
            state = State.CharacterSelection;   
        }
    }
    
    private void Start(){
        SceneManager.sceneLoaded += SceneManager_OnSceneLoaded;
        PlayersManager.Instance.OnAllPlayersReady += PlayersManager_OnAllPlayersReady;
    }

    private void Update(){

        switch (this.state){
            default:
                break;
            case State.CountDown:
                countDownTimer -= Time.deltaTime;
                if (countDownTimer < 0f){
                    state = State.Playing;
                }
                break;
        }

    }

    public void SpawnPlayer(PlayerInput playerInput){


        playerInput.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        playerInput.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        Transform playerSpawnPoint = playerInput.transform;
        PlayerSpawn[] playerSpawns = FindObjectsByType<PlayerSpawn>(FindObjectsSortMode.None);

        foreach (PlayerSpawn playerSpawn in playerSpawns){
            if (playerInput.playerIndex == playerSpawn.GetPlayerKey()){
                playerSpawnPoint = playerSpawn.GetComponent<Transform>();
                break;
            }
        }

        if (playerInput.playerIndex == 0){
            playerInput.transform.GetComponentInChildren<PlayerVisual>(true).SetIsFacingRight(true);
        } else {
            playerInput.transform.GetComponentInChildren<PlayerVisual>(true).SetIsFacingRight(false);
        }

        playerInput.transform.position = playerSpawnPoint.position;
    }

    public bool IsCountingDown(){
        return state == State.CountDown;
    }    

    public bool IsInCharacterSelection(){
        return state == State.CharacterSelection;
    }

    public bool IsGamePlaying(){
        return state == State.Playing;
    }

    public float GetCountDownTimer(){
        return countDownTimer;
    }

    private void StartCountDown(){
        OnCountDownStarted?.Invoke(this, EventArgs.Empty);
        state = State.CountDown;
        countDownTimer = countDownTimerMax;
    }

    private void DisplayStage(){
        if (FindAnyObjectByType<StageBar>() != null){
            FindAnyObjectByType<StageBar>().ColorStageBar(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void PlayersManager_OnAllPlayersReady(object sender, EventArgs e){
        StartCountDown();
    }

    private void SceneManager_OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if (scene.name == Loader.Scene.CharacterSelectionScreen.ToString()){
            return;
        }

        if (scene.name == Loader.Scene.WinScreen.ToString()){
            OnGameOver?.Invoke(this, EventArgs.Empty);
            state = State.GameOver;
            return;
        }

        if (scene.name == Loader.Scene.StartStage.ToString() && state == State.GameOver){
            StartCountDown();  
            PlayersManager.Instance.DisplayPlayers();
        }
        
        DisplayStage();
        SpawnPlayer(PlayersManager.Instance.GetPlayer1Input());
        SpawnPlayer(PlayersManager.Instance.GetPlayer2Input());
    }

    public void TogglePauseGame(){
        if (state == State.Playing){
            Time.timeScale = 0f;
            state = State.Paused;
        } else if (state == State.Paused){
            Time.timeScale = 1f;
            state = State.Playing;
        }
    }

    public bool IsPaused(){
        return state == State.Paused;
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }
}
