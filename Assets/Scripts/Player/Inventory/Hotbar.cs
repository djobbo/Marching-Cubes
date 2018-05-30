using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : Inventory {

    public int selectedSlotID = 0;
    public HotbarSlot selectedSlot;

    public Transform selectedSlotHighlighter;
    public Vector3 offset;

    public Transform itemInHandContainer;

    private void Start()
    {
        SelectSlot(selectedSlotID);
    }

    private void Update()
    {
        var mouseWheel = Input.GetAxis("Mouse ScrollWheel");

        if (mouseWheel > 0f)
        {
            SelectPreviousSlot();
        }
        else if (mouseWheel < 0f)
        {
            SelectNextSlot();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectPreviousSlot();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectNextSlot();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectSlot(5);
        }
    }

    public void SelectNextSlot()
    {
        if (selectedSlot)
            selectedSlot.Select(false);
        selectedSlotID += 7;
        selectedSlotID %= 6;
        selectedSlot = (HotbarSlot)slots[selectedSlotID];
        UpdateSlotHighlighter();
        selectedSlot.Select(true);
    }

    public void SelectPreviousSlot()
    {
        if (selectedSlot)
            selectedSlot.Select(false);
        selectedSlotID += 5;
        selectedSlotID %= 6;
        selectedSlot = (HotbarSlot)slots[selectedSlotID];
        UpdateSlotHighlighter();
        selectedSlot.Select(true);
    }

    public void SelectSlot(int _slotID)
    {
        if (selectedSlot)
            selectedSlot.Select(false);
        selectedSlotID = Mathf.Clamp(_slotID, 0, slots.Count);
        selectedSlot = (HotbarSlot)slots[selectedSlotID];
        UpdateSlotHighlighter();
        selectedSlot.Select(true);
    }

    void UpdateSlotHighlighter()
    {
        selectedSlotHighlighter.position = selectedSlot.transform.position + offset;
    }

}
