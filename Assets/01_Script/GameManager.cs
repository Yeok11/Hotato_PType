using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

enum TileType
{
    None,
    Attack,
    Defense,
    Heal
}


public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] GameObject player;

    [SerializeField] GameObject[] haveTiles; // ��
    public GameObject tilesParent;
    GameObject[] nowTiles = new GameObject[25];

    [SerializeField] GameObject normalTile;

    public TextMeshProUGUI moveCntSign;

    [SerializeField]List<GameObject> settedTile = new List<GameObject>();
     
    TileType nextPlayerState = TileType.None;
    [SerializeField] bool playerTurn;

    [SerializeField] Enemy enemy;

    public Transform poolManager;

    public GameObject footPoint;

    public Vector2 playerLastPos;

    int playerStartPos;
    int playerStartDice;



    internal bool[] playerCanMove = 
    {
        true, true, true, true, true,
        true, true, true, true, true,
        true, true, true, true, true,
        true, true, true, true, true,
        true, true, true, true, true
    };


    private void Start()
    {
        RandomTile();
    }

    //Ű���� �̵� ���� ����
    public bool ProtectKeyboardMove(int nextMove)
    {
        int a = Data.isPlayerTile + nextMove;
        if (a <= 24 && a >= 0 && playerCanMove[a]) return true;

        return false;
    }

    //���콺 �̵� ���� ����
    public bool ProtectDragMove(GameObject tile)
    {
        int a = Data.isPlayerTile;
        if (a < 24 && tile == settedTile[a + 1] && playerCanMove[a + 1]) return true;
        if (a > 0 && tile == settedTile[a - 1] && playerCanMove[a - 1]) return true;
        if (a <= 19 && tile==settedTile[a + 5] && playerCanMove[a + 5]) return true;
        if (a > 4 && tile == settedTile[a - 5] && playerCanMove[a - 5]) return true;

        return false;
    }

    public void SpawnFootPoint()
    {
        Instantiate(footPoint, tilesParent.transform.GetChild(Data.isPlayerTile).position, Quaternion.identity).transform.parent = poolManager;
    }

    public void JudgeEnemy()
    {
        //��¼�� ��¼�� ���ʹ� �׼�
        Debug.Log("���� ��ȣ�ۿ�");
    }

    public void JudgePlayerPos()
    {
        Vector3 pos = playerLastPos;
        playerCanMove[Data.isPlayerTile] = false;

        if (pos.x > player.transform.position.x) Data.isPlayerTile--;
        else if (pos.x < player.transform.position.x) Data.isPlayerTile++;
        else if (pos.y > player.transform.position.y) Data.isPlayerTile += 5;
        else if (pos.y < player.transform.position.y) Data.isPlayerTile -= 5;

        playerLastPos = player.transform.position;
    }

    void Update()
    {
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
        // Ÿ�� ������ �޾ƿ� �Ϳ��� Ÿ�ϵ� ��ġ ����
        for (int i = 0; i < 25; i++)
        {
            settedTile.Add(Instantiate(nowTiles[i], tilesParent.transform.GetChild(i)));
            settedTile[i].transform.parent = tilesParent.transform.GetChild(i);
        }

        if (Data.stageTurn == 1)
        {
            player.transform.position = tilesParent.transform.GetChild(12).position; // �÷��̾� �߾����� ��ġ
            Data.isPlayerTile = 12;
            playerLastPos = player.transform.position;
        }
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

        // �������� �ǹ� ���� Ÿ�Ϸ� ä��� �ڵ�
        for (int i = 0; i < nowTiles.Length; i++) if (nowTiles[i] == null) nowTiles[i] = normalTile;

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
            }

            playerStartDice = Data.canMoveCnt;
            playerStartPos = Data.isPlayerTile;
            CanMoveReset();
            playerTurn = true;
        }
    }

    public void MoveReset()
    {
        Data.isPlayerTile = playerStartPos;
        Data.canMoveCnt = playerStartDice;
        player.transform.position = settedTile[Data.isPlayerTile].transform.position;
        playerLastPos = player.transform.position;
        CanMoveReset();
    }

    void CanMoveReset()
    {
        for (int i = 0; i < playerCanMove.Length; i++) playerCanMove[i] = true;
        for (int i = poolManager.childCount; i > 0; i--) Destroy(poolManager.GetChild(i - 1).gameObject);
    }

    public void EndPlayerTurn()
    {
        int tileValue = nowTiles[Data.isPlayerTile].GetComponent<Tile>().Act();
        nextPlayerState = nowTiles[Data.isPlayerTile].GetComponent<Tile>().tileType;

        if (nextPlayerState == TileType.Attack)
        {
            enemy.Hp -= tileValue;
            Debug.Log($"���� {tileValue}��ŭ ���ظ� �Ծ����ϴ�.");
        }

        nextPlayerState = TileType.None;
        if (enemy.Hp <= 0)
        {
            StageManager.nowStage += 1;
            SceneManager.LoadScene(0);   
        }

        JudgeEnemy();
    }
}
