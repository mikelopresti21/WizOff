using UnityEngine;
using UnityEngine.InputSystem;

public class Spell : MonoBehaviour
{
    public enum SpellType {
        Reverse,
        Slow,
        Damage,
    }
    [SerializeField] private Color effectColor;
    [SerializeField] private SpellSO spellSO;
    private PlayerInput spellCaster;
    public Rigidbody2D spellRB;
    public bool movingRight;
    private float spellSpeed = 15f; 
    private float spellTimer;
    private float spellTimerMax = 5f;
    private Animator spellAnimator;
    private SpellType spellType;
    private float damageAmount;

    private void Awake(){
        spellRB = GetComponent<Rigidbody2D>();
        spellAnimator = GetComponent<Animator>();
        effectColor = spellSO.spellEffectColor;
        spellType = spellSO.spellType;
        damageAmount = spellSO.damageAmount;
    }
    void Update()
    {
        spellTimer += Time.deltaTime;
        if (spellTimer > spellTimerMax){
            Destroy(gameObject);
        }

        if (spellRB.linearVelocityX > 0){
            movingRight = true;
        } else if (spellRB.linearVelocityX < 0){
            movingRight = false;
        }

        spellAnimator.SetFloat("velocityX", Mathf.Sign(spellRB.linearVelocityX));
    }

    public Color GetColor(){
        return effectColor;
    }

    public SpellSO GetSpellSO(){
        return spellSO;
    }

    public void SetSpellCaster(PlayerInput spellCaster){
        this.spellCaster = spellCaster;
    }

    public PlayerInput GetSpellCaster(){
        return spellCaster;
    }

    public Rigidbody2D GetSpellRB(){
        return spellRB;
    }

    public SpellType GetSpellType(){
        return spellType;
    }

    public float GetDamageAmount(){
        return damageAmount;
    }

    void OnTriggerEnter2D(Collider2D trigger){

        if (trigger.GetComponent<Reflector>() != null){
        
            if (movingRight){
                spellRB.linearVelocityX = -spellSpeed;
            } else {
                spellRB.linearVelocityX = spellSpeed;
            }
            
            spellRB.linearVelocityY = -spellRB.linearVelocityY;
            SetSpellCaster(trigger.GetComponent<PlayerInput>());

        }
    }

    void OnCollisionEnter2D(Collision2D collision){

        if (collision.gameObject.GetComponent<PlayerInput>() != spellCaster){
            if (collision.gameObject.GetComponent<Reflector>() == null){
                Destroy(gameObject);
            }        
        }
    }

}
