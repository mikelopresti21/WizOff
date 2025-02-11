using System;
using UnityEngine;

public class WeakChain : MonoBehaviour
{
    public static event EventHandler OnAnyChainBreak;
    private void Start(){
        Detonator.OnAnyDetonatorUsed += Detonator_OnAnyDetonatorUsed;
    }

    public void DestroyChain(){
        OnAnyChainBreak?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);       
    }

    private void Detonator_OnAnyDetonatorUsed(object sender, EventArgs e){
        DestroyChain();
    }

    private void OnCollisionEnter2D(Collision2D collision){

        if (collision.gameObject.GetComponent<Spell>() != null){
            DestroyChain();
        }
    }

    private void OnDestroy() {
        Detonator.OnAnyDetonatorUsed -= Detonator_OnAnyDetonatorUsed;
    }
}
