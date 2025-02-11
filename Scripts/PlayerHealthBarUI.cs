using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private PlayerHealth playerHealth;


    private void Start(){
        
        playerHealth.OnHealthChanged += PlayerHealth_OnHealthChanged;
        healthBar.gameObject.SetActive(false);
    }

    private void Update(){
        if (healthBar.fillAmount < 1){
            healthBar.gameObject.SetActive(true);
        } else {
            healthBar.gameObject.SetActive(false);
        }
    }

    private void PlayerHealth_OnHealthChanged(object sender, PlayerHealth.OnHealthChangedEventArgs e){
        healthBar.fillAmount = e.healthNormalized;
    }
}
