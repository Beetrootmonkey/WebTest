using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static int isMouseOverUI = 0;
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOverUI++;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOverUI--;
    }

    public static bool IsMouseOverUI()
    {
        return isMouseOverUI > 0;
    }
}
