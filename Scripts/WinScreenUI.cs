using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WinScreenUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winMessage;
    [SerializeField] private Image winnerCharacter;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake(){
        restartButton.onClick.AddListener(() => {
            Loader.Restart();
        });

        mainMenuButton.onClick.AddListener(() => {
            Loader.LoadCharacterSelectionScreen();
        });
    }
    void Start()
    {   
        winnerCharacter.sprite = PlayersManager.Instance.GetWinnerSprite();
        int winnerPlayerIndex = PlayersManager.Instance.GetWinnerIndex();
        winMessage.SetText("Player " + (winnerPlayerIndex + 1) + " Wins!");
    }
}
