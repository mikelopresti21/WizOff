using System;
using System.Collections;
using NUnit.Framework.Interfaces;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;
    public class OnHealthChangedEventArgs {
        public float healthNormalized;
    }
    public event EventHandler OnPlayerDied;
    public event EventHandler OnTakeDamage;
    [SerializeField] private PlayerUnsubscribe playerUnsubscribe;
    [SerializeField] private Rigidbody2D playerRB;
    private PlayerCollisions playerCollisions;
    private PlayerTriggers playerTriggers;
    private float health;
    private float healthMax = 100f;
    public bool isDead;
    public bool isInvincible;

    void Start()
    {
        health = healthMax;
        playerRB = GetComponent<Rigidbody2D>();
        playerCollisions = GetComponent<PlayerCollisions>();
        playerTriggers = GetComponent<PlayerTriggers>();
        playerCollisions.OnSpellCollided += PlayerSpells_OnSpellCollided;
        playerCollisions.OnCollidedWithDamage += PlayerCollisions_OnCollidedWithDamage;
        playerTriggers.OnTriggerWithDamage += PlayerTriggers_OnTriggerWithDamage;
        playerUnsubscribe.OnUnsubscribeFromEvents += PlayerUnsubscribe_OnUnsubscribeFromEvents;
        PlayersManager.Instance.OnPlayerReset += PlayersManager_OnPlayerReset;
    }

    void Update()
    {
        if (health <= 0 && !isDead){
            StartCoroutine(Die());
        }   
    }

    public void Damage(float damageAmount){
        if (isInvincible){
            return;
        }

        health -= damageAmount;
        OnTakeDamage?.Invoke(this, EventArgs.Empty);
        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs {
            healthNormalized = health / healthMax
        });
    }

    IEnumerator Die(){
        isDead = true;
        isInvincible = true;
        playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(.5f);
        OnPlayerDied?.Invoke(this, EventArgs.Empty);

        StartCoroutine(IsNoLongerInvincible());
        isDead = false;
        health = healthMax;
        GameManager.Instance.SpawnPlayer(GetComponent<PlayerInput>());
    }

    IEnumerator IsNoLongerInvincible(){
        yield return new WaitForSeconds(2f);
        isInvincible = false;
    }

    private void ResetHealth(){
        health = healthMax;
        isInvincible = false;
        isDead = false;
        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs {
            healthNormalized = 1f
        });
    }

    private void UnsubscribeFromEvents(){
        playerCollisions.OnSpellCollided -= PlayerSpells_OnSpellCollided;
        playerCollisions.OnCollidedWithDamage -= PlayerCollisions_OnCollidedWithDamage;
        playerTriggers.OnTriggerWithDamage -= PlayerTriggers_OnTriggerWithDamage;
        playerUnsubscribe.OnUnsubscribeFromEvents -= PlayerUnsubscribe_OnUnsubscribeFromEvents;
        PlayersManager.Instance.OnPlayerReset += PlayersManager_OnPlayerReset;
    }

    private void PlayersManager_OnPlayerReset(object sender, EventArgs e){
        ResetHealth();
    }

    private void PlayerUnsubscribe_OnUnsubscribeFromEvents(object sender, EventArgs e){
        UnsubscribeFromEvents();
    }

    private void PlayerSpells_OnSpellCollided(object sender, PlayerCollisions.OnSpellCollidedEventArgs e){
        Damage(e.damageAmount);
    }

    private void PlayerCollisions_OnCollidedWithDamage(object sender, PlayerCollisions.OnCollidedWithDamageEventArgs e){
        Damage(e.damageAmount);
    }

    private void PlayerTriggers_OnTriggerWithDamage(object sender, PlayerTriggers.OnTriggerWithDamageEventArgs e){
        Damage(e.damageAmount);
    }
}
