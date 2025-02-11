using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClipRefSO audioClipRefSO;

    private void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    private void Start(){
        Bomb.OnAnyBombExplode += Bomb_OnAnyBombExplode;
        PlayerSpells.OnAnySpellCasted += PlayerSpells_OnAnySpellCasted;
        Bow.OnAnyBowShot += Bow_OnAnyBowShot;
        WeakChain.OnAnyChainBreak += WeakChain_OnAnyChainBreak;
    }

    public void PlaySoundEffect(AudioClip audioClip, Vector3 position){
        AudioSource.PlayClipAtPoint(audioClip, position);
    }

    private void WeakChain_OnAnyChainBreak(object sender, EventArgs e){
        WeakChain weakChain = sender as WeakChain;
        PlaySoundEffect(audioClipRefSO.chainBreak, weakChain.transform.position);
    }

    private void Bow_OnAnyBowShot(object sender, EventArgs e){
        Bow bow = sender as Bow;
        PlaySoundEffect(audioClipRefSO.bow, bow.transform.position);
    }

    private void PlayerSpells_OnAnySpellCasted(object sender, PlayerSpells.OnAnySpellCastedEventArgs e){
        PlayerSpells playerSpells = sender as PlayerSpells;
        PlaySoundEffect(audioClipRefSO.spells[e.spellIndex], playerSpells.transform.position);
    }

    private void Bomb_OnAnyBombExplode(object sender, EventArgs e){
        Bomb bomb = sender as Bomb;
        PlaySoundEffect(audioClipRefSO.bombExplosion, bomb.transform.position);
    }

    public void DestroySelf(){
        Instance = null;
        Destroy(gameObject);
    }
}
