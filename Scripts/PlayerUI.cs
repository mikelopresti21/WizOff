using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private UnityEngine.UI.Image reflectCoolDownBar;
    [SerializeField] private Animator reflectCoolDownBarAnimator;
    [SerializeField] private UnityEngine.UI.Image spellCoolDownBar;
    [SerializeField] private PlayerReflect playerReflect;
    [SerializeField] private PlayerSpells playerSpells;

    private void Start(){

        RectTransform rectTransform = GetComponent<RectTransform>();
        if (playerInput.playerIndex == 0){
            rectTransform.localPosition = new Vector3(-750, -450, 0);
        } else {
            rectTransform.localPosition = new Vector3(750, -450, 0);
        }

        playerReflect.OnRefelectCoolDownChanged += PlayerReflect_OnRefelectCoolDownChanged;
        playerSpells.OnPlayerSpellCoolDown += PlayerReflect_OnPlayerSpellCoolDown;
    }

    private void Update(){
        
        reflectCoolDownBarAnimator.SetBool("IsCoolingDown", !playerReflect.IsReady());
        
    }

    private void PlayerReflect_OnPlayerSpellCoolDown(object sender, PlayerSpells.OnPlayerSpellCoolDownChangedEventArgs e){
        spellCoolDownBar.fillAmount = e.progressNormalized;
    }

    private void PlayerReflect_OnRefelectCoolDownChanged(object sender, PlayerReflect.OnRefelectCoolDownChangedEventArgs e){
        reflectCoolDownBar.fillAmount = e.progressNormalized;
    }


}
