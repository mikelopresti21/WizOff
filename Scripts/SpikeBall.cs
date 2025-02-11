using UnityEngine;

public class SpikeBall : MonoBehaviour, IDealsDamage
{
    private float damageAmount = 100;
    public float GetDamageAmount(){
        return damageAmount;
    }
}
