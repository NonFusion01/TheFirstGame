using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHold : CoreMonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public bool isHolding;
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.isHolding = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(TouchEnd());
    }

    protected IEnumerator TouchEnd()
    {
        yield return new WaitForSeconds(0.05f);
        this.isHolding = false;
    }

}
