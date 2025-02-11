using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Detonator : Item
{
    public static event EventHandler OnAnyDetonatorUsed;

    public override void UseItem(){

        OnAnyDetonatorUsed?.Invoke(this, EventArgs.Empty);

        DestroySelf();
    }

    public void DestroyDetonator(){
        Destroy(gameObject);
    }
}
