using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] int tileValue;

    [SerializeField] internal TileType tileType;

    GameObject player;

    public void Awake()
    {
        player = FindObjectOfType<Player>().gameObject;
    }

    public int Act()
    {
        return tileValue;
    }

    private void OnMouseEnter()
    {
        if (Player.playerMove && Data.canMoveCnt != 0)
        {
            if (GameManager.Instance.ProtectDragMove(gameObject))
            {
                GameManager.Instance.SpawnFootPoint(); //발판 소환
                player.transform.position = transform.position;
                GameManager.Instance.JudgePlayerPos();

                Data.canMoveCnt--;
            }
        }
    }
}