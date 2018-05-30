using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerActions : MonoBehaviour {

    public Hotbar hotbar;
    public PlayerHealth playerHealth;
    public TerrainShaper terrainShaper;
    public Thrower thrower;
    public Camera cam;

    public InventorySlot selectedSlot;
    Item item;
	
	void Update () {

        selectedSlot = hotbar.selectedSlot;
        item = selectedSlot.item;
        if (item != null)
            switch (item.itemType)
            {
                case Item.ItemType.Consumable:
                    Regen();
                    break;
                case Item.ItemType.Gun:
                    Shoot();
                    break;
                case Item.ItemType.Melee:
                    break;
                case Item.ItemType.Bow:
                    break;
                case Item.ItemType.Throwable:
                    ThrowItem();
                    break;
                case Item.ItemType.Shaper:
                    ShapeTerrain();
                    break;
                default:
                    break;
            }
	}

    void ShapeTerrain()
    {
        terrainShaper.ShapeTerrain();
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Shoot !");
            RaycastHit hit;
            if (Physics.Raycast(cam.ScreenPointToRay(new Vector3(Screen.width / 2f,Screen.height / 2f, 0)), out hit, item.range))
            {
                var hitTr = hit.transform;
                if (hitTr == transform)
                {
                    Debug.LogError("We hit ourselves :(");
                    return;
                }
                if (hitTr.CompareTag("Player"))
                {
                    hitTr.GetComponent<PlayerHealth>().TakeDamage(item.damage);
                }
                else if (hitTr.CompareTag("DestructibleObject"))
                {
                    hitTr.GetComponent<DestructibleObject>().TakeDamage(item.damage);
                }
            }
        }
    }

    void Regen()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Regen !");
            playerHealth.Regen(item.healthRegen, item.hungerRegen, item.thirstRegen, item.staminaRegen);
            selectedSlot.Remove(1);
        }
    }

    void ThrowItem() {
        if (Input.GetMouseButtonDown(0))
            thrower.ThrowItem(hotbar.selectedSlot.item);
    }
}
