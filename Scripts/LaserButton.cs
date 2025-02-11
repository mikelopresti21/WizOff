using System.Collections;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class LaserButton : MonoBehaviour, IDealsDamage
{
    public GameObject childPrefab;
    public Transform childSpawn;
    public GameObject laser;
    public bool isPressed;
    public Animator buttonAnimator;
    private float damageAmount = 100f;
    void Start()
    {
        Detonator.OnAnyDetonatorUsed += Detonator_OnAnyDetonatorUsed;
    }

    void Update()
    {
        
    }

    public void ActivateLaser(){
        laser = Instantiate(childPrefab, childSpawn.position, Quaternion.identity);
        isPressed = true;
        buttonAnimator.SetBool("isPressed", isPressed);
        StartCoroutine(DestroyLaser());
    }
    void OnCollisionEnter2D(Collision2D collision){
        if (!isPressed) {
            ActivateLaser();
        }
    }

    IEnumerator DestroyLaser(){
        yield return new WaitForSeconds(3f);
        Destroy(laser);
        laser = null;
        isPressed = false;
        buttonAnimator.SetBool("isPressed", isPressed);
    }
    
    public float GetDamageAmount(){
        return damageAmount;
    }

    private void Detonator_OnAnyDetonatorUsed(object sender, EventArgs e){
        Destroy(gameObject);
    }
}
