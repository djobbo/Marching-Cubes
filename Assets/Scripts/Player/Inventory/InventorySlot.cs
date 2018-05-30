using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public Item item;
    public int count;

    public Image itemSprite;
    public Text itemCountTxt;

    private void Start()
    {
        UpdateUI();
    }

    public void SetItem(Item _item, int _count = 0)
    {
        item = _item;
        count = _count;

        UpdateUI();
    }

    public int Add(int _amount)
    {
        count += _amount;
        var spare = count;
        count = Mathf.Clamp(count, 0, item.maxStackCount);
        Debug.Log("Added " + _amount);
        UpdateUI();
        //Debug.Log(spare > item.maxStackCount);
        if (spare > item.maxStackCount) return spare - item.maxStackCount;
        else return 0;
    }

    public int Remove(int _amount)
    {
        count -= _amount;
        var remains = count;
        count = Mathf.Clamp(count, 0, item.maxStackCount);
        Debug.Log("Removed " + _amount);
        if (count <= 0)
            EmptySlot();
        UpdateUI();

        if (remains <= 0) return -remains;
        else return 0;
    }

    public void SetCount(int _count)
    {
        count = _count;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (item == null)
        {
            itemSprite.gameObject.SetActive(false);
            itemCountTxt.gameObject.SetActive(false);
        }
        else
        {
            itemSprite.gameObject.SetActive(true);
            itemSprite.sprite = item.sprite;
            if (count > 1)
            {
                itemCountTxt.gameObject.SetActive(true);
                itemCountTxt.text = count.ToString();
            }
            else
                itemCountTxt.gameObject.SetActive(false);
        }
    }

    public virtual void EmptySlot()
    {
        item = null;
        count = 0;
    }

}
