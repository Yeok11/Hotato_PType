using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public int Hp;
    public int Power;

    private void Update()
    {
        GetComponentInChildren<TextMeshProUGUI>().SetText("Life = " + Hp);
    }
}
