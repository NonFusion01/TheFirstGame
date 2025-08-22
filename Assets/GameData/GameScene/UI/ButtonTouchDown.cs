using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTouchDown : CoreMonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Button Jump is pressed!");
        CharManager.Instance._charController.charMovement.CharJump();
    }
}
