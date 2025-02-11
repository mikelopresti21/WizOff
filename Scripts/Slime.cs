using UnityEngine;
using UnityEngine.InputSystem;

public class Slime : MonoBehaviour, IDealsDamage
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] int directionInt;
    [SerializeField] private Vector2 direction;
    [SerializeField] private Animator animator;
    private float damageAmount = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        directionInt = UnityEngine.Random.Range(0, 2);
        direction = (directionInt == 0) ? Vector2.left : Vector2.right;
        animator.SetBool("movingRight", direction == Vector2.right);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }

    public float GetDamageAmount(){
        return damageAmount;
    }

    void OnCollisionEnter2D(Collision2D collision){
        PlayerInput playerCollided = collision.gameObject.GetComponent<PlayerInput>();

        if (collision.gameObject.CompareTag("Ground") || playerCollided != null){
            return;
        } else {
            SwitchDirection();
        }

        
    }

    void OnTriggerEnter2D(Collider2D trigger){

        if (trigger.CompareTag("PlayerAdvance") || trigger.CompareTag("PlayerFinish")){
            SwitchDirection();
        }
    }

    public void SwitchDirection(){
        direction = (direction == Vector2.right) ? Vector2.left : Vector2.right;
        animator.SetBool("movingRight", direction == Vector2.right);
    }

    
}
