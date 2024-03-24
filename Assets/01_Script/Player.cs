using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static bool playerMove;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && Data.isPlayerTile > 4) Move(new Vector3(0, 1.7f), -5);
        if (Input.GetKeyDown(KeyCode.A) && Data.isPlayerTile > 0) Move(new Vector3(-1.7f, 0), -1);
        if (Input.GetKeyDown(KeyCode.S) && Data.isPlayerTile <= 19) Move(new Vector3(0, -1.7f), 5);
        if (Input.GetKeyDown(KeyCode.D) && Data.isPlayerTile < 24) Move(new Vector3(1.7f, 0f), 1);
    }

    void Move(Vector3 movement, int moveValue)
    {
        if (Data.canMoveCnt != 0 && GameManager.Instance.ProtectKeyboardMove(moveValue))
        {
            transform.position += movement;
            GameManager.Instance.SpawnFootPoint();
            GameManager.Instance.JudgePlayerPos();
            Data.canMoveCnt--;
        }
    }

    private void OnMouseDrag()
    {
        playerMove = true;
        Debug.Log(playerMove);
    }

    private void OnMouseUp()
    {
        playerMove = false;
    }
}
