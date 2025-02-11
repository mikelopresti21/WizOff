using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI ready;
    [SerializeField] private TextMeshProUGUI notReady;
    [SerializeField] private Image displayedCharacter;
    [SerializeField] private Sprite[] characters;
    [SerializeField] private RuntimeAnimatorController[] animatorControllers;
    [SerializeField] private int characterIndex = 0;
    private int playerIndex;

    void Start(){
        notReady.gameObject.SetActive(true);
        ready.gameObject.SetActive(false);
    }

    public void SetTitle(){
        if (title != null){
            title.SetText("Player " + (playerIndex + 1));
        }
    }

    public void NextCharacter(){
        if (characterIndex == characters.Length - 1){
            characterIndex = 0;
        } else {
            characterIndex ++;
        }
        displayedCharacter.sprite = characters[characterIndex]; 
    }

    public void PrevCharacter(){
        if (characterIndex == 0){
            characterIndex = characters.Length - 1;
        } else {
            characterIndex --;
        }
        
        displayedCharacter.sprite = characters[characterIndex]; 
    }

    public void ShowReady(){
        notReady.gameObject.SetActive(false);
        ready.gameObject.SetActive(true);
    }

    public void HideReady(){
        notReady.gameObject.SetActive(true);
        ready.gameObject.SetActive(false);
    }

    public Sprite GetCurrentSprite(){
        return displayedCharacter.sprite;
    }
    public RuntimeAnimatorController GetCurrentAnimator(){
        return animatorControllers[characterIndex];
    }

    internal void SetPlayerIndex(int playerIndex){
        this.playerIndex = playerIndex;
    }
}
