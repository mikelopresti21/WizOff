using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCollisions : MonoBehaviour
{
    public event EventHandler<OnCollidedWithDamageEventArgs> OnCollidedWithDamage;
    public class OnCollidedWithDamageEventArgs {
        public float damageAmount;
    }
    public event EventHandler<OnSpellCollidedEventArgs> OnSpellCollided;
    public class OnSpellCollidedEventArgs {
        public SpellSO spellSO;
        public Spell.SpellType spellType;
        public float damageAmount;
    }


    void OnCollisionEnter2D(Collision2D collision){


        if (collision.gameObject.TryGetComponent(out Bomb bomb)){
            return;
        } else if (collision.gameObject.TryGetComponent(out IDealsDamage collisionWithDamage)){
            OnCollidedWithDamage?.Invoke(this, new OnCollidedWithDamageEventArgs {
                damageAmount = collisionWithDamage.GetDamageAmount()
            });
        }


        Spell collidedSpell = collision.gameObject.GetComponent<Spell>();

        if (collidedSpell != null && collidedSpell.GetSpellCaster() != GetComponent<PlayerInput>()){

            OnSpellCollided?.Invoke(this, new OnSpellCollidedEventArgs {
                spellSO = collidedSpell.GetSpellSO(),
                spellType = collidedSpell.GetSpellType(),
                damageAmount = collidedSpell.GetDamageAmount()
            });
        }
    }

    void OnCollisionStay2D(Collision2D collision){

        if (collision.gameObject.TryGetComponent(out Bomb bomb)){
            return;
        } else if (collision.gameObject.TryGetComponent(out IDealsDamage collisionWithDamage)){
            OnCollidedWithDamage?.Invoke(this, new OnCollidedWithDamageEventArgs {
                damageAmount = collisionWithDamage.GetDamageAmount()
            });
        }
    }
}
