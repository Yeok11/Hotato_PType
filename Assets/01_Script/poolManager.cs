using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poolManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnPos;
    [SerializeField] GameObject[] spawnData;
    [SerializeField] int[] spawnCnt;

    private void Awake()
    {
        for (int i = 0; i < spawnData.Length; i++)
        {
            for (int j = 0; j < spawnCnt[i]; j++)
            {
                Instantiate(spawnData[i]).transform.parent = spawnPos[i];
            }
        }
    }
}
