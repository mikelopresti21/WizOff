using System;
using UnityEngine;

public class PlayerTriggers : MonoBehaviour
{
    public event EventHandler<OnTriggerWithDamageEventArgs> OnTriggerWithDamage;
    public class OnTriggerWithDamageEventArgs {
        public float damageAmount;
    }
    void OnTriggerEnter2D(Collider2D trigger){

        if (trigger.TryGetComponent(out Bomb bomb)){
            if (!bomb.IsExploded()){
                return;
            }
        }
        
        if (trigger.TryGetComponent(out IDealsDamage triggerWithDamage)){
            OnTriggerWithDamage?.Invoke(this, new OnTriggerWithDamageEventArgs{
                damageAmount = triggerWithDamage.GetDamageAmount()
            });
        }
    }

    void OnTriggerStay2D(Collider2D trigger){
        
        if (trigger.TryGetComponent(out Bomb bomb)){
            if (!bomb.IsExploded()){
                return;
            }
        }
        
        if (trigger.TryGetComponent(out IDealsDamage triggerWithDamage)){
            OnTriggerWithDamage?.Invoke(this, new OnTriggerWithDamageEventArgs{
                damageAmount = triggerWithDamage.GetDamageAmount()
            });
        }
    }
}
