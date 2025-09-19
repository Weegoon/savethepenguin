using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laddle : MonoBehaviour
{
    public GameObject LaddleObj;

    public Vector3 OriginalPos;

    public float AddedValue;

    public void PlaceLaddle()
    {
        Vector3 v = LaddleObj.transform.position;
        v.y = OriginalPos.y + AddedValue * UIController.instance.Levelplay.scoreIndex;
        LaddleObj.transform.position = v;
    }
}
