using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item))]
public class CustomItemEditor : Editor {

    public override void OnInspectorGUI()
    {
        Item item = (Item) target;

        item.name = EditorGUILayout.TextField("Item: ", item.name);
        item.id = EditorGUILayout.IntField("ID: ", item.id);
        item.maxStackCount = EditorGUILayout.IntField("Max Stack Count: ", item.maxStackCount);

        item.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite: ", item.sprite, typeof(Sprite), false);

        item.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab: ", item.prefab, typeof(GameObject), false);

        item.droppedItemPrefab = (DroppedItem)EditorGUILayout.ObjectField("Dropped Item Prefab: ", item.droppedItemPrefab, typeof(DroppedItem), false);

        item.itemInHandPrefab = (GameObject)EditorGUILayout.ObjectField("Item In Hand Prefab: ", item.itemInHandPrefab, typeof(GameObject), false);

        EditorGUILayout.Space();

        item.itemType = (Item.ItemType)EditorGUILayout.EnumPopup("Item Type: ", item.itemType);

        if (item.itemType == Item.ItemType.Consumable)
        {
            item.healthRegen = EditorGUILayout.FloatField("Health Regen: ", item.healthRegen);
            item.hungerRegen = EditorGUILayout.FloatField("Hunger Regen: ", item.hungerRegen);
            item.thirstRegen = EditorGUILayout.FloatField("Thirst Regen: ", item.thirstRegen);
            item.staminaRegen = EditorGUILayout.FloatField("Stamina Regen: ", item.staminaRegen);
        }
        else if (item.itemType == Item.ItemType.Shaper)
        {

        }
        else
        {
            item.damage = EditorGUILayout.FloatField("Damage: ", item.damage);
            item.maxAmmo = EditorGUILayout.IntField("Max Ammo: ", item.maxAmmo);
            item.range = EditorGUILayout.FloatField("Range: ", item.range);
        }
    }

}
