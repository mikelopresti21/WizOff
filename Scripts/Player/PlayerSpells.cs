using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerSpells : MonoBehaviour
{
    public static event EventHandler<OnAnySpellCastedEventArgs> OnAnySpellCasted;
    public class OnAnySpellCastedEventArgs {
        public int spellIndex;
    }
    public event EventHandler<OnPlayerSpellCoolDownChangedEventArgs> OnPlayerSpellCoolDown;

    public class OnPlayerSpellCoolDownChangedEventArgs {
        public float progressNormalized;
    }
    private const string CAST_SPELL = "CastSpell";
    [SerializeField] private PlayerInput playerInput; 
    [SerializeField] private PlayerUnsubscribe playerUnsubscribe;   
    [SerializeField] private SpellListSO spellListSO;
    [SerializeField] private PlayerMoveController playerMoveController;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [SerializeField] private PlayerHealth playerHealth;
    private float spellSpeed = 20f;
    private float spellCoolDownTimerMax = 1f;
    private float spellCoolDownTimer;
    private bool isReadyToCastSpell;
    private int spellIndex;

    void Start(){
        
        PlayersManager.Instance.OnAllPlayersReady += PlayersManager_OnAllPlayersReady;
        playerHealth.OnPlayerDied += PlayerHealth_OnPlayerDied;
        playerUnsubscribe.OnUnsubscribeFromEvents += PlayerUnsubscribe_OnUnsubscribeFromInputs;

    }

    private void Update(){

        if (!isReadyToCastSpell){
            spellCoolDownTimer -= Time.deltaTime;
            OnPlayerSpellCoolDown?.Invoke(this, new OnPlayerSpellCoolDownChangedEventArgs {
                progressNormalized = 1 - spellCoolDownTimer/spellCoolDownTimerMax
            });
            if (spellCoolDownTimer < 0f){
                isReadyToCastSpell = true;
            }
        }

    }

    private void CastSpell_performed(InputAction.CallbackContext context){
        if (!GameManager.Instance.IsGamePlaying()){
            return;
        }
        
        if (isReadyToCastSpell){

            spellCoolDownTimer = spellCoolDownTimerMax;
            isReadyToCastSpell = false;
            OnPlayerSpellCoolDown?.Invoke(this, new OnPlayerSpellCoolDownChangedEventArgs {
                progressNormalized = 0f
            });

            Transform spellSpawn = right;
            if (playerRB.linearVelocityX > 0){
                spellSpawn = right;
            } else if (playerRB.linearVelocityX < 0){
                spellSpawn = left;
            }

            Vector2 spellDirection = playerMoveController.GetMoveInput();
            GameObject spellPrefab = spellListSO.spellSOList[spellIndex].spellPrefab;
            Spell castedSpell = Instantiate(spellPrefab, spellSpawn.position, Quaternion.Euler(0, 0, Mathf.Atan2(spellDirection.y, spellDirection.x) * Mathf.Rad2Deg)).GetComponent<Spell>();
            
            castedSpell.SetSpellCaster(playerInput);
            castedSpell.GetSpellRB().linearVelocity = spellDirection * spellSpeed;

            OnAnySpellCasted?.Invoke(this, new OnAnySpellCastedEventArgs {
                spellIndex = spellIndex
            });

            if (spellIndex == spellListSO.spellSOList.Count - 1){
                spellIndex = 0;
            } else {
                spellIndex ++;
            }
        }
    }

    private void UnsubscribeFromEvents(){
        PlayersManager.Instance.OnAllPlayersReady -= PlayersManager_OnAllPlayersReady;
        playerHealth.OnPlayerDied -= PlayerHealth_OnPlayerDied;
        playerUnsubscribe.OnUnsubscribeFromEvents -= PlayerUnsubscribe_OnUnsubscribeFromInputs;
    }

    private void PlayerUnsubscribe_OnUnsubscribeFromInputs(object sender, EventArgs e){
        DisableControls();
        UnsubscribeFromEvents();
    }

    private void PlayerHealth_OnPlayerDied(object sender, EventArgs e){
        spellCoolDownTimer = 0f;
        isReadyToCastSpell = true;
        OnPlayerSpellCoolDown?.Invoke(this, new OnPlayerSpellCoolDownChangedEventArgs {
            progressNormalized = 1f
        });
    }
    private void PlayersManager_OnAllPlayersReady(object sender, EventArgs e){
        EnableControls();
    }

    private void EnableControls(){
        playerInput.actions[CAST_SPELL].performed += CastSpell_performed;
    }

    private void DisableControls(){
        playerInput.actions[CAST_SPELL].performed -= CastSpell_performed;
    }
}
