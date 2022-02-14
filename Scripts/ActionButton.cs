using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Player player;

    public void OnPointerDown(PointerEventData eventData)
    {
        player.HasFired = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        player.HasFired = false;
    }

}
