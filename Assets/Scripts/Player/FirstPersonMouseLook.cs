using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class FirstPersonMouseLook : MonoBehaviour {

    public Inventory inventory;

    Vector2 mouseLook;
    Vector2 smoothV;
    public float sensivity = 5.0f;
    public float smoothing = 2.0f;

    Transform character;

    [Header("Other")]
    public ItemTooltip itemTooltip;

    private void Start()
    {
        character = transform.parent;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        md = Vector2.Scale(md, new Vector2(sensivity * smoothing, sensivity * smoothing));

        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        mouseLook += smoothV;

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.Rotate(Vector3.up * smoothV.x);

        var showTooltip = false;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0)), out hit))
        {
            var hitTr = hit.transform;

            if (hitTr.CompareTag("DroppedItem"))
            {
                showTooltip = true;
                var droppedItem = hitTr.GetComponent<DroppedItem>();
                itemTooltip.SetText(droppedItem.item.name.ToUpper());
                itemTooltip.transform.position = hitTr.position;
                itemTooltip.transform.LookAt(2 * itemTooltip.transform.position - transform.position);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    inventory.AddItem(droppedItem.item, droppedItem.count);
                    Destroy(hitTr.gameObject);
                }
            }
        }

        itemTooltip.gameObject.SetActive(showTooltip);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

}
