using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    public static PauseMenuUI Instance { get; private set; }
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private EventSystem eventSystem;

    private void Awake(){

        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
             
    }
    
    private void Start(){

        if (eventSystem == null) {
            if (FindAnyObjectByType<EventSystem>() != null){
                eventSystem = FindAnyObjectByType<EventSystem>();
                eventSystem.firstSelectedGameObject = resumeButton.gameObject;
                DontDestroyOnLoad(eventSystem.gameObject);
            } 
        }   

        resumeButton.onClick.AddListener(() => {
            GameManager.Instance.TogglePauseGame();
            Hide();
        });

        mainMenuButton.onClick.AddListener(() => {
            Loader.LoadCharacterSelectionScreen();
        });

        PlayerPause.OnAnyPlayerPause += PlayerPause_OnAnyPlayerPause;
        Hide();
    }

    private void Show(){
        eventSystem.firstSelectedGameObject = resumeButton.gameObject;
        gameObject.SetActive(true);
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void PlayerPause_OnAnyPlayerPause(object sender, EventArgs e){
        GameManager.Instance.TogglePauseGame();
        if (GameManager.Instance.IsPaused()){
            Show();
        } else {
            Hide();
        }
    }

    public void DestroySelf(){
        PlayerPause.OnAnyPlayerPause -= PlayerPause_OnAnyPlayerPause;
        Destroy(eventSystem.gameObject);
        Destroy(gameObject);
    }

    internal void DestroyEventSystem(){
        Destroy(eventSystem.gameObject);
        eventSystem = null;
    }
}
