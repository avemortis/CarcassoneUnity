using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class Tile : MonoBehaviour
{   //ID Тайла
    int TileID;

    //Наличие мипа на тайле
    public bool MipsHaving = false;

    //Наличие флажка
    public bool Flag = false;

    public byte[] ZoneZone = new byte[25] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    public bool[] MipsSloteSlote = new bool[25] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };

    //Отображает зоны, имеющиеся на тайле. Поле(1); Дорога(2);  Город(3);  Монастырь (4); Перекресток (5)
    public byte[,] Zone = new byte[5, 5] { { 0, 0, 0, 0, 0 },
                                           { 0, 0, 0, 0, 0 },
                                           { 0, 0, 0, 0, 0 },
                                           { 0, 0, 0, 0, 0 },
                                           { 0, 0, 0, 0, 0 } };

    //Отображение зон для размещение мипса
    public bool[,] MipsSlote = new bool[5, 5] {{ false, false, false, false, false },
                                               { false, false, false, false, false },
                                               { false, false, false, false, false },
                                               { false, false, false, false, false },
                                               { false, false, false, false, false } };
    
    //Позиция на доске

    public void ConvertToMatrix()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Zone[i, j] = ZoneZone[i * 5 + j];
                MipsSlote[i, j] = MipsSloteSlote[i * 5 + j];
            }
        }
        
    }

    public byte[,] GetZone ()
    {
        return Zone;
    }

    public void SetIdentity(byte[,] Zone, bool[,] MipsSlote)
    {
        this.Zone = Zone;
        this.MipsSlote = MipsSlote;
    }

    //Возвращает возможность сопоставления с новым тайлом. 1 - слева 2 - справа 3 - сверху 4 - снизу
    public bool MatchingCheck(byte[,] NewTileZone, byte SideNewTile) 
    {

        if (SideNewTile == 1) // Если слева
        {
            if (this.Zone[2, 0] == NewTileZone[2, 4])
                return true;
            else
                return false;
        }
        if (SideNewTile == 2) // Если справа
        {
            if (this.Zone[2, 4] == NewTileZone[2, 0])
                return true;
            else
                return false;
        }
        if (SideNewTile == 3) //Если сверху
        {
            if (this.Zone[0, 2] == NewTileZone[4, 2])
                return true;
            else
                return false;
        }
        if (SideNewTile == 4) //Если снизу
        {
            if (this.Zone[4, 2] == NewTileZone[0, 2])
                return true;
            else
                return false;
        }
        else return false;
    }

    void cyclic_roll(byte a, byte b, byte c, byte d)
    {
        byte temp = a;
        a = b;
        b = c;
        c = d;
        d = temp;
    }

    void cyclic_roll(bool a, bool b, bool c, bool d)
    {
        bool temp = a;
        a = b;
        b = c;
        c = d;
        d = temp;
    }

    //Поворот тайла вправо
    public void RotateTileRight() 
    {
        byte n = 5;
        for (int i = 0; i < n / 2; i++)
        {
            for (int j = 0; j < (n + 1) / 2; j++)
            {
                //cyclic_roll(Zone[i, j], Zone[n - 1 - j, i], Zone[n - 1 - i, n - 1 - j], Zone[j, n - 1 - i]);
                byte temp = Zone[i, j];
                Zone[i, j] = Zone[n - 1 - j, i];
                Zone[n - 1 - j, i] = Zone[n - 1 - i, n - 1 - j];
                Zone[n - 1 - i, n - 1 - j] = Zone[j, n - 1 - i];
                Zone[j, n - 1 - i] = temp;

                bool tempbool = MipsSlote[i, j];
                MipsSlote[i, j] = MipsSlote[n - 1 - j, i];
                MipsSlote[n - 1 - j, i] = MipsSlote[n - 1 - i, n - 1 - j];
                MipsSlote[n - 1 - i, n - 1 - j] = MipsSlote[j, n - 1 - i];
                MipsSlote[j, n - 1 - i] = tempbool;

            }
        }
    }
};
