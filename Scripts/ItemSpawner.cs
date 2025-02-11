using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance { get; private set; }
    [SerializeField] private GameObject[] commonItems;
    [SerializeField] private GameObject[] rareItems;
    [SerializeField] private Transform itemSpawnPoint;
    private Item itemInPlay;
    private float itemSpawnnTimer = 1f;
    private float itemSpawnTimerMax = 10f;

    private void Awake(){
        Instance = this;
    }
    private void Start(){
        itemInPlay = FindAnyObjectByType<Item>();
        Item.OnAnyItemDestroyed += Item_OnItemDestroyed;
    }
    private void Update()
    {
        itemSpawnnTimer -= Time.deltaTime;
        if (itemSpawnnTimer < 0f){
            if (itemInPlay == null){
                SpawnItem();
            }
            itemSpawnnTimer = itemSpawnTimerMax;
        }
    }

    private void SpawnItem(){
        
        GameObject selectedItem;
        if (UnityEngine.Random.Range(0f, 100f) <= 90){
            selectedItem = commonItems[UnityEngine.Random.Range(0, commonItems.Length)];
        } else {
            selectedItem = rareItems[UnityEngine.Random.Range(0, rareItems.Length)];
        }

        itemInPlay = Instantiate(selectedItem, itemSpawnPoint.position, quaternion.identity).GetComponent<Item>();

    }

    private void Item_OnItemDestroyed(object sender, EventArgs e){
        itemInPlay = null;
    }

    private void OnDestroy(){
        Item.OnAnyItemDestroyed -= Item_OnItemDestroyed;
    }

    public Item GetItemInPlay(){
        return itemInPlay;
    }
}
