using UnityEngine;

public class Lightning : MonoBehaviour, IDealsDamage
{
    private float damageAmount = 2.5f;

    public void DestroyLightning(){
        Destroy(gameObject);
    }

    public float GetDamageAmount(){
        return damageAmount;
    }
}
