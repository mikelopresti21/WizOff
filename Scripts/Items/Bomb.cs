using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : Item, IDealsDamage
{
    private const string IS_DETONATED = "isDetonated";
    public static event EventHandler OnAnyBombExplode;
    [SerializeField] private Animator bombAnimator;
    [SerializeField] private new ParticleSystem particleSystem;
    private float explosionRadius = 10f;
    private bool hasDetonated;
    private bool isExploded;
    private float detonationTimer;
    private float detonationTimerMax = 2f;
    private float explosionDamage = 100f;
    
    void Update()
    {
        if (isThrown) {
            if (!hasDetonated){
                DetonateBomb();
            }
            detonationTimer += Time.deltaTime;
            if (detonationTimer > detonationTimerMax){
                StartCoroutine(Explode());
            }
        }
    }

    private void DetonateBomb(){
        hasDetonated = true;
        bombAnimator.SetBool(IS_DETONATED, true); 
    }

    public bool IsExploded(){
        return isExploded;
    }

    IEnumerator Explode(){
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        isExploded = true;
        particleSystem.Play();
        CircleCollider2D explosionTrigger = gameObject.AddComponent<CircleCollider2D>();
        explosionTrigger.isTrigger = true;
        explosionTrigger.radius = explosionRadius;
        OnAnyBombExplode?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(1f);
        DestroySelf();
    }

    protected override void OnCollisionEnter2D(Collision2D collision){

        Spell collidedSpell = collision.gameObject.GetComponent<Spell>();
        if (collidedSpell != null){
            switch (collidedSpell.GetSpellType()){

                case Spell.SpellType.Reverse:
                    GetComponent<Rigidbody2D>().linearVelocityX = -GetComponent<Rigidbody2D>().linearVelocityX;
                    break;

                case Spell.SpellType.Slow:
                    GetComponent<Rigidbody2D>().linearVelocityX = 0.5f * GetComponent<Rigidbody2D>().linearVelocityX;
                    break;

                case Spell.SpellType.Damage:
                    StartCoroutine(Explode());
                    break;
            }
        }
    }

    public float GetDamageAmount(){
        return explosionDamage;
    }
}
