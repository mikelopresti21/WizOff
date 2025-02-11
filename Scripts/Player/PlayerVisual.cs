using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private const string IS_FACING_RIGHT = "isFacingRight"; 
    [SerializeField] private PlayerUnsubscribe playerUnsubscribe;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SpriteRenderer playerSR;
    [SerializeField] private Color normalColor = new Color(1f, 1f, 1f);
    [SerializeField] private Color hurtColor = new Color(0.8f, 0f, 0f);
    [SerializeField] private PlayerCollisions playerCollisions;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerHealth playerHealth;
    private bool isFacingRight;
    private float colorChangeTimer;
    private float colorChangeTimerMax = 3f;
    private bool isEffectedBySpell;
    private float hurtTimer;
    private float hurtTimerMax = 0.25f;
    private bool isHurt;
    private Color curEffectColor;
    
    private void Start(){

        playerCollisions.OnSpellCollided += playerSpells_OnSpellCollided;
        playerHealth.OnTakeDamage += PlayerHealth_OnTakeDamage;
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
        GameManager.Instance.OnCountDownStarted += GameManager_OnCountDownStarted;
        playerUnsubscribe.OnUnsubscribeFromEvents += PlayerUnsubscribe_OnUnsubscribeFromInputs;
        PlayersManager.Instance.OnPlayerReset += PlayersManager_OnPlayerReset;
        HidePlayerVisual();

    }

    private void Update()
    {
        if (isEffectedBySpell){
            playerSR.color = curEffectColor;
            colorChangeTimer -= Time.deltaTime;
            if (colorChangeTimer < 0f){
                isEffectedBySpell = false;
                curEffectColor = normalColor;
                ResetColor();
            }
        }

        if (isHurt){
            playerSR.color = hurtColor;
            hurtTimer -= Time.deltaTime;
            if (hurtTimer < 0f){
                isHurt = false;
                if (!isEffectedBySpell){
                    ResetColor();
                }
            }
        }
        if (rb.linearVelocityX > 0){
            isFacingRight = true;
        } else if (rb.linearVelocityX < 0){
            isFacingRight = false;
        }

        playerAnimator.SetBool(IS_FACING_RIGHT, isFacingRight);
    }

    private void GameManager_OnCountDownStarted(object sender, EventArgs e){
        ShowPlayerVisual();
    }

    private void GameManager_OnGameOver(object sender, EventArgs e){
        HidePlayerVisual();
    }

    public void SetIsFacingRight(bool isFacingRight){
        this.isFacingRight = isFacingRight;
        playerAnimator.SetBool(IS_FACING_RIGHT, isFacingRight);
    }

    private void ChangePlayerSpriteColorFromSpell(Color effectColor){
        curEffectColor = effectColor;
        isEffectedBySpell = true;
        colorChangeTimer = colorChangeTimerMax;
    }

    private void ResetColor(){
        playerSR.color = normalColor;
    }

    private void ResetPlayerVisual(){
        isEffectedBySpell = false;
        colorChangeTimer = 0f;
        isHurt = false;
        hurtTimer = 0f;
        ResetColor();
    }

    private void UnsubscribeFromEvents(){
        playerCollisions.OnSpellCollided -= playerSpells_OnSpellCollided;
        playerHealth.OnTakeDamage -= PlayerHealth_OnTakeDamage;
        GameManager.Instance.OnGameOver -= GameManager_OnGameOver;
        GameManager.Instance.OnCountDownStarted -= GameManager_OnCountDownStarted;
        playerUnsubscribe.OnUnsubscribeFromEvents -= PlayerUnsubscribe_OnUnsubscribeFromInputs;
    }

    private void PlayersManager_OnPlayerReset(object sender, EventArgs e){
        ResetPlayerVisual();
    }

    private void PlayerUnsubscribe_OnUnsubscribeFromInputs(object sender, EventArgs e){
        UnsubscribeFromEvents();
    }

    public void playerSpells_OnSpellCollided(object sender, PlayerCollisions.OnSpellCollidedEventArgs e){
        if (e.spellType != Spell.SpellType.Damage){
            ChangePlayerSpriteColorFromSpell(e.spellSO.spellEffectColor);
        }
    }

    public void PlayerHealth_OnTakeDamage(object sender, EventArgs e){
        isHurt = true;
        hurtTimer = hurtTimerMax;
    }

    public void SetPlayerSprite(Sprite sprite){
        playerSR.sprite = sprite;
    }

    public Sprite GetPlayerSprite(){
        return playerSR.sprite;
    }

    internal void SetRuntimeAnimatorController(RuntimeAnimatorController runtimeAnimatorController){
        playerAnimator.runtimeAnimatorController = runtimeAnimatorController;
    }

    public void HidePlayerVisual(){
        gameObject.SetActive(false);
    }

    public void ShowPlayerVisual(){
        gameObject.SetActive(true);
    }
}
