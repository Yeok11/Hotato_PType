using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_01 : MonoBehaviour
{
    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }
}