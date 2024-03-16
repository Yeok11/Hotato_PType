using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

enum TileType
{
    None,
    Attack,
    Defense
}


public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] haveTiles; // ��
    GameObject[] nowTiles = new GameObject[25];
    [SerializeField] GameObject normalTile;
    public GameObject tilesParent;
    public TextMeshProUGUI moveCntSign;

    TileType nextPlayerState = TileType.None;

    [SerializeField]Enemy enemy;
    int tileValue;



    [SerializeField] bool playerTurn;


    private void Start()
    {
        RandomTile();
    }


    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha0)) RandomTile();

        moveCntSign.SetText(Data.canMoveCnt.ToString());

        if (playerTurn && Data.canMoveCnt == 0) // �÷��̾� �� ����
        {
            Debug.Log("�� ����");
            playerTurn = false;
            EndPlayerTurn();
        }
    }

    void TileSet()
    {
        if (tilesParent.transform.GetChild(0).childCount != 0) // ������ �ִ� Ÿ�ϵ� ��� ����
        {
            for (int i = 0; i < tilesParent.transform.childCount; i++)
            {
                Destroy(tilesParent.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
        
        // Ÿ�� ������ �޾ƿ� �Ϳ��� Ÿ�ϵ� ��ġ ����
        for (int i = 0; i < 25; i++)
        {
            GameObject a = Instantiate(nowTiles[i]);
            a.transform.parent = tilesParent.transform.GetChild(i);

            a.transform.position = a.transform.parent.position;
        }

        if (Data.stageTurn == 1)
        {
            player.transform.position = tilesParent.transform.GetChild(12).position; // �÷��̾� �߾����� ��ġ
            Data.isPlayerTile = 12;
        }

        Debug.Log("Ÿ�� �� �Ϸ�");
    }

    public void RandomTile()
    {
        nowTiles = new GameObject[25];

        List<int> randomDeckOrder = RandomShuffle(haveTiles.Length); // ���� �ִ� Ÿ�ϵ��� ���� ����
        List<int> randomTileOrder = RandomShuffle(nowTiles.Length); // �� Ÿ���� ���� ����
        
        for (int i = 0; i < 25; i++)
        {
            if (i >= haveTiles.Length) break;
            nowTiles[randomTileOrder[i]] = haveTiles[randomDeckOrder[i]];
        }

        for (int i = 0; i < nowTiles.Length; i++) // �������� �ǹ� ���� Ÿ�Ϸ� ä��� �ڵ�
        {
            if (nowTiles[i] == null) nowTiles[i] = normalTile;
        }

        TileSet();
    }

    List<int> RandomShuffle(int range)
    {
        List<int> x = new List<int>();

        for (int i = 0; i < range; i++) x.Insert(UnityEngine.Random.Range(0, x.Count), i);

        return x;
    }

    public void RollDice()
    {
        if (!playerTurn)
        {
            Data.canMoveCnt = 0; // Ȥ�ó� ī��Ʈ ������ ������ �ʱ�ȭ

            for (int i = 0; i < Data.diceCnt; i++)
            {
                int cnt = UnityEngine.Random.Range(1, 7);
                Data.canMoveCnt += cnt;

                Debug.Log(i + 1 + "�� ���̽��� ���� : " + cnt);
            }

            player.GetComponent<Player>().PlayerMoveSet();

            playerTurn = true;
        }
    }

    public void EndPlayerTurn()
    {
        

        tileValue = nowTiles[Data.isPlayerTile].GetComponent<Tile>().Act();
        nextPlayerState = nowTiles[Data.isPlayerTile].GetComponent<Tile>().tileType;

        if (nextPlayerState == TileType.Attack)
        {
            enemy.Hp -= tileValue;
            Debug.Log($"���� {tileValue}��ŭ ���ظ� �Ծ����ϴ�.");
        }

        nextPlayerState = TileType.None;
    }
}
