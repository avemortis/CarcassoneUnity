using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Board : MonoBehaviour
{
    //Board
    public byte[,] GamingBoard = new byte[350, 350];
    public bool[,] MipsOnBoard = new bool[350, 350];
    //Meeps
    public struct Coord
    {
        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;
    }
    //public Stack<Coord> PointStack = new Stack<Coord>();
    public GameObject Meep;
    public List<GameObject> AllMeeps = new List<GameObject>();

    //Размещает Zone по координатам на игровой доске. Координаты в формате тайла.
    public void SetTileOnBoard(byte[,] NewTileZone, int iPlace, int jPlace)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j< 5; j++)
            {
                GamingBoard[iPlace * 5 + i, jPlace * 5 + j] = NewTileZone[i, j];
            }
        }
    }
    public void SetTileOnBoard(GameObject NewTile, int iPlace, int jPlace)
    {
        Tile TempZone = NewTile.GetComponent<Tile>();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GamingBoard[iPlace * 5 + i, jPlace * 5 + j] = TempZone.Zone[i, j];
            }
        }
    }
    //Вывод матрицы
    public void MatrixCheck()
    {
        for (int i = 0; i < 350; i++)
        {
            for (int j = 0; j < 350; j++)
            {
                if (GamingBoard[i, j] > 0)
                    Debug.Log(GamingBoard[i, j]);
            }
        }
    }

    public bool MatchingCheck(byte[,] NewTileZone, int x, int y)
    {
        x = x * 5;
        y = y * 5;
        if ((GamingBoard[x + 2, y - 1] == NewTileZone[2, 0] || GamingBoard[x + 2, y - 1] == 0) && (GamingBoard[x - 1, y + 2] == NewTileZone[0, 2] || GamingBoard[x - 1, y + 2] == 0) && (GamingBoard[x + 2, y + 5] == NewTileZone[2, 4] || GamingBoard[x + 2, y + 5] == 0) && (GamingBoard[x + 5, y + 2] == NewTileZone[4, 2] || GamingBoard[x + 5, y + 2] == 0))
            return true;
        else return false;
    }

    public bool MatchingCheck(GameObject NewTile, int x, int y)
    {
        Tile TempZone = NewTile.GetComponent<Tile>();
        x = x * 5 + 2;
        y = y * 5 + 2;
        if ((GamingBoard[x - 3, y] == TempZone.Zone[0, 2] || GamingBoard[x - 3, y] == 0) && (GamingBoard[x, y + 3] == TempZone.Zone[2, 4] || GamingBoard[x, y + 3] == 0) && (GamingBoard[x + 3, y] == TempZone.Zone[4, 2] || GamingBoard[x + 3, y] == 0) && (GamingBoard[x, y - 3] == TempZone.Zone[2, 0] || GamingBoard[x, y - 3] == 0))
            return true;
        else return false;
    }

    //Проверка соседних клеток
    bool NeighborsCheck(int x, int y, byte NeighborTypeFind) 
    {
        if ((GamingBoard[x - 1, y] == NeighborTypeFind) || (GamingBoard[x + 1, y] == NeighborTypeFind) || (GamingBoard[x, y - 1] == NeighborTypeFind) || (GamingBoard[x, y + 1] == NeighborTypeFind))
            return true;
        else
            return false;
    }
    //Проверка перехода на новый тайл
    bool TileCrossCheck(int x, int y, int x1, int y1)
    {
        if ((x / 5 != x1 / 5) | (y / 5 != y1 / 5))
        {
            return true;
        }
        else return false;
    }
    //Почистить доску
    public void CleanBoard()
    {
        for (int i = 0; i < 350; i++)
        {
            for (int j = 0; j < 350; j++)
            {
                GamingBoard[i, j] = 0;
                MipsOnBoard[i, j] = false;
            }
        }
    }
    public void SetMeepOnBoard(int matrixX, int matrixY, Vector3 sceneposition)
    {
        Meep TempMeep = Meep.GetComponent<Meep>();
        MipsOnBoard[matrixX, matrixY] = true;
        TempMeep.SetMark(matrixX, matrixY);
        Meep.transform.position = sceneposition;
        Meep.transform.Translate(0, 0, -1);
        AllMeeps.Add(Instantiate(Meep));
    }

    //"Проталкивание" стэка структуры
    public void Push (int i, int j)
    {
        Stack<Coord> PointStack = new Stack<Coord>();
        Stack<Coord> ToAdd = new Stack<Coord>();
        Coord FirstPoint = new Coord(i, j);
        PointStack.Push(FirstPoint);
        bool StillGrowing = true;
        while (StillGrowing == true)
        {
            int Size = PointStack.Count;
            foreach(Coord Point in PointStack)
            {
                //UP
                if (GamingBoard[Point.X , Point.Y] == GamingBoard[Point.X - 1 , Point.Y])
                {
                    bool AlreadyCount = false;
                    foreach (Coord CheckPoint in PointStack)
                    {
                        if (CheckPoint.X == Point.X - 1 && CheckPoint.Y == Point.Y)
                        {
                            AlreadyCount = true;
                            break;
                        }
                    }
                    foreach (Coord CheckPoint in ToAdd)
                    {
                        if (CheckPoint.X == Point.X - 1 && CheckPoint.Y == Point.Y)
                        {
                            AlreadyCount = true;
                            break;
                        }
                    }
                    if (AlreadyCount == false)
                    {
                        Coord NewPoint = new Coord(Point.X - 1, Point.Y);
                        ToAdd.Push(NewPoint);
                    }
                }
                //DOWN
                if (GamingBoard[Point.X, Point.Y] == GamingBoard[Point.X + 1, Point.Y])
                {
                    bool AlreadyCount = false;
                    foreach (Coord CheckPoint in PointStack)
                    {
                        if (CheckPoint.X == Point.X + 1 && CheckPoint.Y == Point.Y)
                        {
                            AlreadyCount = true;
                            break;
                        }
                    }
                    foreach (Coord CheckPoint in ToAdd)
                    {
                        if (CheckPoint.X == Point.X + 1 && CheckPoint.Y == Point.Y)
                        {
                            AlreadyCount = true;
                            break;
                        }
                    }
                    if (AlreadyCount == false)
                    {
                        Coord NewPoint = new Coord(Point.X + 1, Point.Y);
                        ToAdd.Push(NewPoint);
                    }
                }
                //LEFT
                if (GamingBoard[Point.X, Point.Y] == GamingBoard[Point.X, Point.Y - 1])
                {
                    bool AlreadyCount = false;
                    foreach (Coord CheckPoint in PointStack)
                    {
                        if (CheckPoint.X == Point.X && CheckPoint.Y == Point.Y - 1)
                        {
                            AlreadyCount = true;
                            break;
                        }
                    }
                    foreach (Coord CheckPoint in ToAdd)
                    {
                        if (CheckPoint.X == Point.X && CheckPoint.Y == Point.Y - 1)
                        {
                            AlreadyCount = true;
                            break;
                        }
                    }
                    if (AlreadyCount == false)
                    {
                        Coord NewPoint = new Coord(Point.X, Point.Y - 1);
                        ToAdd.Push(NewPoint);
                    }
                }
                //RIGHT
                if (GamingBoard[Point.X, Point.Y] == GamingBoard[Point.X, Point.Y + 1])
                {
                    bool AlreadyCount = false;
                    foreach (Coord CheckPoint in PointStack)
                    {
                        if (CheckPoint.X == Point.X && CheckPoint.Y == Point.Y + 1)
                        {
                            AlreadyCount = true;
                            break;
                        }
                    }
                    foreach (Coord CheckPoint in ToAdd)
                    {
                        if (CheckPoint.X == Point.X && CheckPoint.Y == Point.Y + 1)
                        {
                            AlreadyCount = true;
                            break;
                        }
                    }
                    if (AlreadyCount == false)
                    {
                        Coord NewPoint = new Coord(Point.X, Point.Y + 1);
                        ToAdd.Push(NewPoint);
                    }
                }
            }
            if (ToAdd.Count == 0)
                StillGrowing = false;
            foreach (Coord Point in ToAdd)
                PointStack.Push(Point);
            while (ToAdd.Count != 0)
                ToAdd.Pop();
        }
        foreach (Coord Point in PointStack)
        {
            Debug.Log("X: " + Point.X + " Y: " + Point.Y + "\n");
        }
    }

    public void JustCheck()
    {
        Push(170, 170);
    }

    //Алгоритм поиска
    bool Search (int xStart, int yStart, byte TypeToSearh, byte HowManyNeed)
    {
        return true;
    }


     //Поле(1); Дорога(2);  Город(3);  Монастырь (4); Перекресток (5)

}
