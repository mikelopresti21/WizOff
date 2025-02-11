using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReflect : MonoBehaviour
{
    private const string REFLECT = "Reflect";
    public event EventHandler<OnRefelectCoolDownChangedEventArgs> OnRefelectCoolDownChanged;
    public class OnRefelectCoolDownChangedEventArgs {
        public float progressNormalized;
    }
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerUnsubscribe playerUnsubscribe;
    [SerializeField] private Transform right;
    [SerializeField] private Transform left;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private GameObject reflectPrefab;
    [SerializeField] private PlayerHealth playerHealth;
    private Transform reflectPos;
    private Reflector reflector;
    private float maxReflectDuration = 3f;
    private float curReflectDuration;
    private float coolDownTimer = 0f;
    private float coolDownTimerMax = 2f;
    private bool isReflecting;
    private bool isReady = true;

    void Start(){
        curReflectDuration = maxReflectDuration;
        PlayersManager.Instance.OnAllPlayersReady += PlayersManager_OnAllPlayersReady;
        playerHealth.OnPlayerDied += PlayerHealth_OnPlayerDied;
        playerUnsubscribe.OnUnsubscribeFromEvents += PlayerUnsubscribe_OnUnsubscribeFromInputs;
    }

    void Update(){

        if (playerRB.linearVelocityX > 0){
            reflectPos = right;
            if (reflector != null && reflector.IsFlipped()){
                reflector.FlipHorizontally();
                reflector.transform.position = reflectPos.position;     
            }  
        } else if (playerRB.linearVelocityX < 0){
            reflectPos = left;
            if (reflector != null && !reflector.IsFlipped()){
                reflector.FlipHorizontally();  
                reflector.transform.position = reflectPos.position;        
            } 
        }

        if (isReflecting){
            curReflectDuration -= Time.deltaTime;
            OnRefelectCoolDownChanged?.Invoke(this, new OnRefelectCoolDownChangedEventArgs {
                progressNormalized = curReflectDuration/maxReflectDuration
            });
            if (curReflectDuration <= 0f)
            {
                RemoveReflect();
                isReady = false;
                curReflectDuration = maxReflectDuration;
            }
        } 

        if (!isReady){
            coolDownTimer += Time.deltaTime;
            OnRefelectCoolDownChanged?.Invoke(this, new OnRefelectCoolDownChangedEventArgs {
                progressNormalized = coolDownTimer/coolDownTimerMax
            });
            if (coolDownTimer >= coolDownTimerMax){
                isReady = true;
                coolDownTimer = 0f;
            }
        }

        if (isReady && !isReflecting && curReflectDuration < maxReflectDuration){
            curReflectDuration += Time.deltaTime;
            OnRefelectCoolDownChanged?.Invoke(this, new OnRefelectCoolDownChangedEventArgs {
                progressNormalized = curReflectDuration/maxReflectDuration
            });
        }
    }

    private void Reflect_started(InputAction.CallbackContext context){
        if (!GameManager.Instance.IsGamePlaying()){
            return;
        }
        
        Transform reflectPos = null;
        if (playerRB.linearVelocityX > 0){
            reflectPos = right;
        } else if (playerRB.linearVelocityX < 0){
            reflectPos = left;
        }

        if (isReady){
            if (reflector == null){
                reflector = Instantiate(reflectPrefab, reflectPos.position, Quaternion.identity).GetComponent<Reflector>();
            }
            reflector.transform.parent = reflectPos;
            isReflecting = true;
        }
        
    }

    public bool IsReady(){
        return isReady;
    }

    private void Reflect_canceled(InputAction.CallbackContext context){
        if (!GameManager.Instance.IsGamePlaying()){
            return;
        }
        
        RemoveReflect();
    }

    private void RemoveReflect(){
        if (reflector != null){
            reflector.DestroyReflector();
            reflector = null;
        }

        isReflecting = false;
    }

    private void PlayerHealth_OnPlayerDied(object sender, EventArgs e){
        curReflectDuration = 0f;
        curReflectDuration = maxReflectDuration;
        isReady = true;
        OnRefelectCoolDownChanged?.Invoke(this, new OnRefelectCoolDownChangedEventArgs {
            progressNormalized = 1f
        });
    }

    private void UnsubscribeFromEvents(){
        PlayersManager.Instance.OnAllPlayersReady -= PlayersManager_OnAllPlayersReady;
        playerHealth.OnPlayerDied -= PlayerHealth_OnPlayerDied;
        playerUnsubscribe.OnUnsubscribeFromEvents -= PlayerUnsubscribe_OnUnsubscribeFromInputs;
    }

    private void PlayerUnsubscribe_OnUnsubscribeFromInputs(object sender, EventArgs e){
        DisableControls();
        UnsubscribeFromEvents();
    }

    private void PlayersManager_OnAllPlayersReady(object sender, EventArgs e){
        EnableControls();
    }

    private void EnableControls(){
        playerInput.actions[REFLECT].started += Reflect_started;
        playerInput.actions[REFLECT].canceled += Reflect_canceled;
    }

    private void DisableControls(){
        playerInput.actions[REFLECT].started -= Reflect_started;
        playerInput.actions[REFLECT].canceled -= Reflect_canceled;
    }
}
