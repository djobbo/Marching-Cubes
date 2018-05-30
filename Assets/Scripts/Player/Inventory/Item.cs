using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Item : ScriptableObject {

    [Header("Basics")]
    public int id;
    public int maxStackCount = 1;

    [Header("Graphics")]
    public GameObject prefab;
    public Sprite sprite;
    public DroppedItem droppedItemPrefab;
    public GameObject itemInHandPrefab;

    public enum ItemType
    {
        Consumable,
        Gun,
        Melee,
        Bow,
        Throwable,
        Shaper
    }

    public ItemType itemType;

    [Header("Consumable")]
    public float healthRegen = 0f;
    public float hungerRegen = 0f;
    public float thirstRegen = 0f;
    public float staminaRegen = 0f;

    [Header("Weapon")]
    public float damage = 0f;
    public int maxAmmo = 0;
    public float range;
}
