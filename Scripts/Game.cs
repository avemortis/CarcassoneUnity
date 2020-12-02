using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    ////Position
    int Center = 34;
    Vector2 centr = new Vector2(0.0f, 0.0f);
    //Functional
    public Board GamingBoard;
    public GameObject[] TilesFromBox;
    private GameObject ActiveTile;
    public GameObject StartTile;
    public GameObject EmptySlot;
    //UI
    public GameObject ImageUI;
    public Canvas Canvas;
    public GameObject MeepSlot;
    public List<GameObject> MeepsSlots = new List<GameObject>();
    //Game Status
    bool GameWaitingForMeep = false;
    //Board
    float SizeOfTile = 0.9f;
    public List<GameObject> EmptySlots = new List<GameObject>();
    //Player
    public GameObject Meep;
    public List<GameObject> AllMeeps = new List<GameObject>();

    int[] LastSloteMarks = new int[2] { 0, 0 };


    //Старт игры
    public void GameStart()
    {
        GamingBoard.CleanBoard();
        Tile Temp = StartTile.GetComponent<Tile>();
        Temp.ConvertToMatrix();
        GamingBoard.SetTileOnBoard(Temp.Zone, Center, Center);
        StartTile.transform.position = centr;
        Instantiate(StartTile);
        CreateSlotsAfterSettingTile(Center, Center);
        ActiveTile = TakeTileFromBox();
        //GamingBoard.MatrixCheck();
    }

    void CheckAll()
    {
        foreach (GameObject Slot in EmptySlots)
        {
            EmptyTileSlot Temp = Slot.GetComponent<EmptyTileSlot>();
            Debug.Log(Temp.MatrixMark[0]);
        }
    }

    void CreateSlotsAfterSettingTile(int x, int y) //Принимает матричные координаты
    {
        bool Left = true;
        bool Right = true;
        bool Up = true;
        bool Down = true;

        foreach(GameObject Slot in EmptySlots)
        {
            EmptyTileSlot Temp = Slot.GetComponent<EmptyTileSlot>();
            if ((Temp.MatrixMark[0] == x) && (Temp.MatrixMark[1] == y - 1)||(GamingBoard.GamingBoard[(x * 5 + 2), (y * 5 + 2) - 5] != 0))
                Left = false;
            if ((Temp.MatrixMark[0] == x) && (Temp.MatrixMark[1] == y + 1)||(GamingBoard.GamingBoard[(x * 5 + 2), (y * 5 + 2) + 5] != 0))
                Right = false;
            if ((Temp.MatrixMark[0] == x - 1) && (Temp.MatrixMark[1] == y)||(GamingBoard.GamingBoard[(x * 5 + 2) - 5, (y * 5 + 2)] != 0))
                Up = false;
            if ((Temp.MatrixMark[0] == x + 1) && (Temp.MatrixMark[1] == y)||(GamingBoard.GamingBoard[(x * 5 + 2) + 5, (y * 5 + 2)] != 0))
                Down = false;
        }
        if (Left == true)
        {
            EmptyTileSlot Temp = EmptySlot.GetComponent<EmptyTileSlot>();
            Temp.SetMark(x, y - 1);
            EmptySlot.transform.position = new Vector3((y - 1 - Center) * SizeOfTile, -(x - Center) * SizeOfTile, 0.0f);
            EmptySlots.Add(Instantiate(EmptySlot));
        }
        if (Right == true)
        {
            EmptyTileSlot Temp = EmptySlot.GetComponent<EmptyTileSlot>();
            Temp.SetMark(x, y + 1);
            EmptySlot.transform.position = new Vector3((y + 1 - Center) * SizeOfTile, -(x - Center) * SizeOfTile, 0.0f);
            EmptySlots.Add(Instantiate(EmptySlot));
        }
        if (Up == true)
        {
            EmptyTileSlot Temp = EmptySlot.GetComponent<EmptyTileSlot>();
            Temp.SetMark(x - 1, y);
            EmptySlot.transform.position = new Vector3((y - Center) * SizeOfTile, -(x - 1 - Center) * SizeOfTile, 0.0f);
            EmptySlots.Add(Instantiate(EmptySlot));
        }
        if (Down == true)
        {
            EmptyTileSlot Temp = EmptySlot.GetComponent<EmptyTileSlot>();
            Temp.SetMark(x + 1, y);
            EmptySlot.transform.position = new Vector3((y - Center) * SizeOfTile, -(x + 1 - Center) * SizeOfTile, 0.0f);
            EmptySlots.Add(Instantiate(EmptySlot));
        }
    }
    
    public GameObject TakeTileFromBox()
    {
        int rand = Random.Range(0, TilesFromBox.Length - 1);
        ActiveTile = Instantiate(TilesFromBox[rand]);
        Array.Clear(TilesFromBox, rand, 1);
        for (int i = rand + 1; i < TilesFromBox.Length; i++)
        {
            TilesFromBox[i - 1] = TilesFromBox[i];
        }
        Array.Resize(ref TilesFromBox, TilesFromBox.Length - 1);
        Tile Temp = ActiveTile.GetComponent<Tile>();
        Temp.ConvertToMatrix();
        ImageUI.transform.rotation = Quaternion.identity;
        SetImageOnUI(ActiveTile);
        return ActiveTile;
    }

    public void SetImageOnUI(GameObject ObjectToSet)
    {
        Sprite OverWrite = ObjectToSet.GetComponent<SpriteRenderer>().sprite;
        ImageUI.GetComponent<Image>().sprite = OverWrite;
    }

    void TileAlwaysFallowToActiveSlot()
    {
        foreach (GameObject Slot in EmptySlots)
        {
            EmptyTileSlot Temp = Slot.GetComponent<EmptyTileSlot>();
            if (Temp.MouseOnMe == true)
            {
                //Debug.Log("I want to be here");
                ActiveTile.transform.position = Slot.transform.position;
                ActiveTile.transform.Translate(0, 0, 1);
            }
        }
    }

    void SetTileOnActiveSlot()
    {
        for (int i = 0; i < EmptySlots.Count; i++)
        {
            EmptyTileSlot Temp = EmptySlots[i].GetComponent<EmptyTileSlot>();
            if (Temp.MouseOnMe == true)
            {
                if (GamingBoard.MatchingCheck(ActiveTile, Temp.MatrixMark[0], Temp.MatrixMark[1]) == true)
                {
                    GamingBoard.SetTileOnBoard(ActiveTile, Temp.MatrixMark[0], Temp.MatrixMark[1]);
                    LastSloteMarks[0] = Temp.MatrixMark[0];
                    LastSloteMarks[1] = Temp.MatrixMark[1];
                    Destroy(EmptySlots[i]);
                    EmptySlots.RemoveAt(i);
                    CreateSlotsAfterSettingTile(Temp.MatrixMark[0], Temp.MatrixMark[1]);
                    MakeMeepSlots();
                    GameWaitingForMeep = true;
                    //TakeTileFromBox();
                }
            }
        }
    }

    public void MakeMeepSlots()
    {
        Tile TempTile = ActiveTile.GetComponent<Tile>();
        Transform ImageUITransform = ImageUI.GetComponent<Transform>();
        float SizeOfTileOnGUI = ImageUI.GetComponent<RectTransform>().rect.width;
        float SizeOfMeepSlot = SizeOfTileOnGUI / 5;
        for (byte i = 0; i < 5; i++)
        {
            for (byte j = 0; j < 5; j++)
            {
                if (TempTile.MipsSlote[i,j] == true)
                {
                    GameObject Slot = Instantiate(MeepSlot);
                    SlotForMeep2 TempMeepSlot = Slot.GetComponent<SlotForMeep2>();
                    TempMeepSlot.SetMarks(i, j);
                    Slot.transform.position = ImageUI.transform.position;
                    Slot.transform.Translate(-SizeOfTileOnGUI/2, SizeOfTileOnGUI/2, 0);
                    Slot.transform.Translate(SizeOfMeepSlot / 2, -SizeOfMeepSlot / 2, 0);
                    Slot.transform.Translate(SizeOfMeepSlot * j, -SizeOfMeepSlot * i, 0);
                    Slot.transform.SetParent(ImageUITransform);
                    MeepsSlots.Add(Slot);
                }
            }
        }
    }

    void SetMeepOnTile()
    {
        foreach(GameObject MeepSlot in MeepsSlots)
        {
            SlotForMeep2 Temp = MeepSlot.GetComponent<SlotForMeep2>();
            if (Temp.MouseOnMe == true)
            {
                GamingBoard.SetMeepOnBoard(LastSloteMarks[0] * 5 + Temp.x, LastSloteMarks[1] * 5 + Temp.y, ActiveTile.transform.position);
            }
        }
        for (int i = 0; i < MeepsSlots.Count; i ++)
        {
            Destroy(MeepsSlots[i]);
        }
        MeepsSlots.Clear();
        GameWaitingForMeep = false;
        TakeTileFromBox();
    }

    void RotateTile()
    {
        Tile Temp = ActiveTile.GetComponent<Tile>();
        Temp.RotateTileRight();
        ActiveTile.transform.Rotate(0, 0, -90);
        ImageUI.transform.Rotate(0, 0, -90);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameWaitingForMeep == false)
        TileAlwaysFallowToActiveSlot();
        if (Input.GetMouseButtonDown(0))
            if (GameWaitingForMeep == false)
            {
                SetTileOnActiveSlot();
            }
            else
            {
                SetMeepOnTile();
            }
        if (Input.GetMouseButtonDown(2))
            if (GameWaitingForMeep == false)
                RotateTile();
    }
}
