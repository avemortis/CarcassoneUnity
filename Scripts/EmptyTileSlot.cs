using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine.EventSystems;
using UnityEngine;

public class EmptyTileSlot : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    public int[] MatrixMark = new int[2];

    public bool MouseOnMe = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("YES YES YEEEES");
        //MouseOnMe = true;
    }

    public void SetMark(int x, int y)
    {
        MatrixMark[0] = x;
        MatrixMark[1] = y;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Debug.Log("Hello");
        MouseOnMe = true;
        //GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 60);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //Debug.Log("Bye-Bye");
        MouseOnMe = false;
        //GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 150);
    }

    public bool AreYouLookAtMe()
    {
        return MouseOnMe;
    }

    public bool AreYouHere(int x, int y)
    {
        if ((MatrixMark[0] == x) & (MatrixMark[1] == y))
            return true;
        else return false;
    }

    public void JustCheck()
    {
        Debug.Log("Hey-hey-hey");
    }

    public int[] GetMark()
    {
        return MatrixMark;
    }
}
