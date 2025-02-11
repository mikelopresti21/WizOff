using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void DestroySelf(){
        Instance = null;
        Destroy(gameObject);
    }
}
