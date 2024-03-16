using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] int tileValue;

    [SerializeField] internal TileType tileType;

    public int Act()
    {
        return tileValue;
    }
}