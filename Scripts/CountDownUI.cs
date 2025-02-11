using System;
using TMPro;
using UnityEngine;

public class CountDownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownText;
    private int prevNumber = 3;

    private void Start(){
        countDownText.text = prevNumber.ToString();
    }

    private void Update(){
        if (GameManager.Instance.IsCountingDown()){
            int curNumber = (int)Math.Ceiling(GameManager.Instance.GetCountDownTimer());
            if (curNumber != prevNumber){
                countDownText.text = curNumber.ToString();
                prevNumber = curNumber;
            }
        }

        if (GameManager.Instance.IsGamePlaying()){
            Destroy(gameObject);
        }
    }

}
