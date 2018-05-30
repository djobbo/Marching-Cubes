using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public List<InventorySlot> slots;

    public Item testItem;
    public int amount = 1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            AddItem(testItem, amount);
        }
    }

    public void AddItem(Item _item, int _amount)
    {
        var _slots = slots.FindAll(s => s.item == _item);
        foreach (var slot in _slots)
        {
            _amount = slot.Add(_amount);
            if (_amount <= 0) return;
        }
        var i = 0;
        while (_amount > 0 && i < slots.Count)
        {
            if (slots[i].item == null)
            {
                slots[i].SetItem(_item);
                _amount = slots[i].Add(_amount);
            }
            i++;
        }
        if (_amount > 0) Debug.Log("Not Enough Space !");
    }

    public bool AddItemToSlot(Item _item, int _slot, int _amount)
    {
        if (slots[_slot] == null)
        {
            slots[_slot].SetItem(_item, _amount);
            return true;
        }
        else if (slots[_slot].item == _item && slots[_slot].count < _item.maxStackCount - _amount)
        {
            slots[_slot].Add(_amount);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveItemFromSlot(int _slot, int _amount)
    {
        slots[_slot].Add(-_amount);
    }

    public void DropItem(int _slot, int _amount)
    {
        var remainingItemsCount = Mathf.Max(slots[_slot].count - _amount, 0);
        for (int i = slots[_slot].count - 1; i >= remainingItemsCount; i--)
        {
            //Spawn Dropped Item Prefab
        }
        slots[_slot].SetCount(remainingItemsCount);
    }

    public void DropAllItems()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            DropItem(i, slots[i].count);
        }
    }

    void UpdateUI()
    {

    }

}

public class InventoryItem
{
    public Item item;
    public int count;
}
