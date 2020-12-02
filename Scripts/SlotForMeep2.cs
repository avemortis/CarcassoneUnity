using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class SlotForMeep2 : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    //public Sprite Image;
    public Sprite Active;
    public Sprite Deactive;
    public bool MouseOnMe = false;
    public byte x;
    public byte y;

    void Start()
    {
        //Image = GetComponent<Image>().sprite;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        MouseOnMe = true;
        GetComponent<Image>().sprite = Active;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        MouseOnMe = false;
        GetComponent<Image>().sprite = Deactive;
    }
    public void SetMarks(byte x, byte y)
    {
        this.x = x;
        this.y = y;
    }
}
