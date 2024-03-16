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
    [SerializeField] GameObject[] haveTiles; // 덱
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

        if (playerTurn && Data.canMoveCnt == 0) // 플레이어 턴 종료
        {
            Debug.Log("턴 종료");
            playerTurn = false;
            EndPlayerTurn();
        }
    }

    void TileSet()
    {
        if (tilesParent.transform.GetChild(0).childCount != 0) // 기존에 있던 타일들 모두 정리
        {
            for (int i = 0; i < tilesParent.transform.childCount; i++)
            {
                Destroy(tilesParent.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
        
        // 타일 종류를 받아온 것에서 타일들 배치 시작
        for (int i = 0; i < 25; i++)
        {
            GameObject a = Instantiate(nowTiles[i]);
            a.transform.parent = tilesParent.transform.GetChild(i);

            a.transform.position = a.transform.parent.position;
        }

        if (Data.stageTurn == 1)
        {
            player.transform.position = tilesParent.transform.GetChild(12).position; // 플레이어 중앙으로 배치
            Data.isPlayerTile = 12;
        }

        Debug.Log("타일 셋 완료");
    }

    public void RandomTile()
    {
        nowTiles = new GameObject[25];

        List<int> randomDeckOrder = RandomShuffle(haveTiles.Length); // 덱에 있는 타일들의 순서 섞기
        List<int> randomTileOrder = RandomShuffle(nowTiles.Length); // 깔린 타일의 순서 섞기
        
        for (int i = 0; i < 25; i++)
        {
            if (i >= haveTiles.Length) break;
            nowTiles[randomTileOrder[i]] = haveTiles[randomDeckOrder[i]];
        }

        for (int i = 0; i < nowTiles.Length; i++) // 나머지를 의미 없는 타일로 채우는 코드
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
            Data.canMoveCnt = 0; // 혹시나 카운트 오류를 예방한 초기화

            for (int i = 0; i < Data.diceCnt; i++)
            {
                int cnt = UnityEngine.Random.Range(1, 7);
                Data.canMoveCnt += cnt;

                Debug.Log(i + 1 + "번 다이스의 숫자 : " + cnt);
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
            Debug.Log($"적이 {tileValue}만큼 피해를 입었습니다.");
        }

        nextPlayerState = TileType.None;
    }
}
