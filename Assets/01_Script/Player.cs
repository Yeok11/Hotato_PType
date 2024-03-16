using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    

    internal bool[] canMoveTile =
    {
        true, true, true, true, true,
        true, true, true, true, true,
        true, true, true, true, true,
        true, true, true, true, true,
        true, true, true, true, true
    };

    public void PlayerMoveSet()
    {
        for (int i = 0; i < canMoveTile.Length; i++)
        {
            canMoveTile[i] = true;
        }

        canMoveTile[Data.isPlayerTile] = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) Move('L');
        if (Input.GetKeyDown(KeyCode.D)) Move('R');
        if (Input.GetKeyDown(KeyCode.S)) Move('D');
        if (Input.GetKeyDown(KeyCode.W)) Move('U');
    }



    public void Move(char arr)
    {
        if (Data.canMoveCnt != 0)
        {
            if (JudgeCanMove(arr))
            {
                transform.position += new 
                    Vector3(
                    arr == 'L' ? -1.7f : arr == 'R' ? 1.7f : 0,
                    arr == 'D' ? -1.7f : arr == 'U' ? 1.7f : 0
                    );

                Data.isPlayerTile += arr == 'L' ? -1 : arr == 'R' ? 1 : 0;
                Data.isPlayerTile += arr == 'U' ? -5 : arr == 'D' ? 5 : 0;

                canMoveTile[Data.isPlayerTile] = false;
                --Data.canMoveCnt;
            }
        }
    }

    bool JudgeCanMove(char arrow)
    {
        switch (arrow)
        {
            case 'R':
                return Data.isPlayerTile % 5 + 1 < 5 && canMoveTile[Data.isPlayerTile + 1];

            case 'L':
                return Data.isPlayerTile % 5 - 1 > -1 && canMoveTile[Data.isPlayerTile - 1];

            case 'U':
                return Data.isPlayerTile - 5 > -1 && canMoveTile[Data.isPlayerTile - 5];

            case 'D':
                return Data.isPlayerTile + 5 < 25 && canMoveTile[Data.isPlayerTile + 5];
        }

        return false;
    }
}
