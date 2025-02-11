using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private int playerKey;

    public int GetPlayerKey(){
        return playerKey;
    }
}
