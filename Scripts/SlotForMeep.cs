using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine.EventSystems;
using UnityEngine;

public class SlotForMeep : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{

    public SpriteRenderer SpriteRenderer;
    public Sprite Active;
    public Sprite Deactive;
    public bool MouseOnMe = false;
    byte x;
    byte y;

    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        MouseOnMe = true;
        SpriteRenderer.sprite = Active;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        MouseOnMe = false;
        SpriteRenderer.sprite = Deactive;
    }
    public void SetMarks(byte x, byte y)
    {
        this.x = x;
        this.y = y;
    }
}
