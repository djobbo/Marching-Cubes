using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour {

    public Item item;
    public int count;

    public void Setup(Item _item, int _count)
    {
        item = _item;
        count = _count;
    }

}
