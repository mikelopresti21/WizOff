using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageBar : MonoBehaviour
{
    [SerializeField] public GameObject[] stageBars;
    public Color curStageColor;

    public void ColorStageBar(int curScene){
        Image curStage = stageBars[curScene - 1].GetComponent<Image>(); 
        curStage.color = curStageColor; 
    }
}
