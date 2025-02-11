using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bow : Item
{
    public static event EventHandler OnAnyBowShot;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawn;
    public bool flipped;
    public int maxShots = 3;
    public float arrowSpeed = 20f;
    public int shotCount = 0;

    
    void Update()
    {
        if (transform.parent != null){
            bool isRightHand = transform.parent.name == "RightHand";
            if ((isRightHand && flipped) || (!isRightHand && !flipped))
            {
                FlipHorizontally();
            }
        }
        
    }

    public override void UseItem(){
        if (transform.parent.name == "RightHand"){
            
            Arrow arrow = Instantiate(arrowPrefab, arrowSpawn.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.arrowRB.linearVelocity = Vector2.right * arrowSpeed;
        } else {
            
            Arrow arrow = Instantiate(arrowPrefab, arrowSpawn.position, Quaternion.Euler(0f, 0f, 180f)).GetComponent<Arrow>();
            arrow.arrowRB.linearVelocity = Vector2.left * arrowSpeed;
        }
        
        OnAnyBowShot?.Invoke(this, EventArgs.Empty);
        shotCount ++;
        
        if (shotCount >= maxShots){
            DestroySelf();
        }
        
    }

    public void FlipHorizontally(){
        Vector3 scale = transform.localScale;
        scale.x = -scale.x;
        transform.localScale = scale;
        flipped = !flipped;
    }
}
