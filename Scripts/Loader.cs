using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene {
        CharacterSelectionScreen,
        WinScreen,
        StartStage,
    }

    public static void LoadCharacterSelectionScreen(){
        
        MusicManager.Instance.DestroySelf();
        SoundManager.Instance.DestroySelf();
        PauseMenuUI.Instance.DestroySelf();
        GameManager.Instance.DestroySelf();
        PlayersManager.Instance.DestroySelf();
        Time.timeScale = 1f;
        SceneManager.LoadScene(Scene.CharacterSelectionScreen.ToString());
    }

    public static void LoadWinScene(){

        Time.timeScale = 1f;
        SceneManager.LoadScene(Scene.WinScreen.ToString());

    }

    internal static void Restart(){

        Time.timeScale = 1f;
        PlayersManager.Instance.ResetPlayers();
        SceneManager.LoadScene(Scene.StartStage.ToString());
    }
}
