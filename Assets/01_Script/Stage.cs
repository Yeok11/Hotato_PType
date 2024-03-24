using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    [SerializeField] int StageNum;
    public void SceneMove()
    {
        if (StageManager.nowStage <= StageNum)
        {
            StageManager.nowStage = StageNum;
            SceneManager.LoadScene(1);
        }
    }
}
