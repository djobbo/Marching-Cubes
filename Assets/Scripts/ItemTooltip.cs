using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour {

    public Text label;
    public LayoutGroup layout;

    public void SetText(string _txt)
    {
        label.text = _txt;
        layout.enabled = false;
        layout.enabled = true;
    }

}
