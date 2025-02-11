using System.Collections;
using UnityEngine;

public class Boomarang : Item
{
    public PlayerReady thrower;
    public bool isReversed;
    public bool allowCollisions;
    public bool reverseActivated;
    public float maxSpeed = 15f;
    public Vector2 startingDirection;
    [SerializeField] private Rigidbody2D boomarangRB;

    void Update(){
        if (isThrown){
            boomarangRB.linearVelocity = startingDirection * maxSpeed;
            if (!reverseActivated){
                StartCoroutine(ReverseBoomarang());
                StartCoroutine(AllowCollisions());
                reverseActivated = true;
            }

            boomarangRB.AddTorque(1f);
            boomarangRB.gravityScale = 0f;
            if (isReversed){
                boomarangRB.linearVelocity = -startingDirection * maxSpeed;
            }
        }  
    }

    IEnumerator ReverseBoomarang(){
        yield return new WaitForSeconds(0.75f);
        isReversed = true;
    }
    IEnumerator AllowCollisions(){
        yield return new WaitForSeconds(0.1f);
        allowCollisions = true;
    }
}
