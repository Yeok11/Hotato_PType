using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] GameObject[] stages;
    [SerializeField] GameObject player;

    public static int nowStage = 0;

    private void Start()
    {
        player.transform.position = stages[nowStage].transform.position;
    }
}
