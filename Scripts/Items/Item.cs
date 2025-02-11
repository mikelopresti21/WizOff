using System;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static event EventHandler OnAnyItemDestroyed;
    public Rigidbody2D itemRB;
    private bool canPickup;
    public bool isHeld;
    protected bool isThrown;
    
    void Start()
    {
        itemRB = GetComponent<Rigidbody2D>();
        canPickup = true;
    }

    public bool CanPickup(){
        return canPickup;
    }

    public virtual void DestroySelf(){
        OnAnyItemDestroyed?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    public virtual void ThrowItem(){
        isThrown = true;
        isHeld = false;
    }

    public virtual void UseItem(){

    }

    protected virtual void OnCollisionEnter2D(Collision2D collision){

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
                    DestroySelf();
                    break;
            }
        }
    }
}
