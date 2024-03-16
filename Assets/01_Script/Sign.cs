using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    public int stageTurn = 1;

    public int isPlayerTile;

    public int diceCnt = 2;
    public int canMoveCnt = 0;

    void Update()
    {
        isPlayerTile = Data.isPlayerTile;
        stageTurn = Data.stageTurn;
        diceCnt = Data.diceCnt;
        canMoveCnt = Data.canMoveCnt;
    }
}
