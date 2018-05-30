using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarSlot : InventorySlot {

    public Hotbar hotbar;
    public GameObject itemInHand;

    public void Select(bool select)
    {
        if (item == null) return;
        if (itemInHand == null)
        {
            itemInHand = Instantiate(item.itemInHandPrefab, hotbar.itemInHandContainer);
        }
        itemInHand.SetActive(select);
    }

    public override void EmptySlot()
    {
        base.EmptySlot();

        Destroy(itemInHand);
        itemInHand = null;
    }

}
