using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableBlock : WordBlock, IPointerClickHandler
{
    // TODO: what to do exactly?
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("keyword: " + textBox.text);
    }
}
