using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour, IDealsDamage
{
    public Rigidbody2D arrowRB;
    public bool allowCollisions;
    [SerializeField] private float damageAmount = 50f;

    void Start()
    {
        StartCoroutine(AllowCollisions());
    }

    void OnCollisionEnter2D(){
        if (allowCollisions){
            Destroy(gameObject);
        }
    }

    public float GetDamageAmount(){
        return damageAmount;
    }

    IEnumerator AllowCollisions(){
        yield return new WaitForSeconds(0.1f);
        allowCollisions = true;
    }
}
