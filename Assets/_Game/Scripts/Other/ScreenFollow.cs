using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFollow : MonoBehaviour
{

    void Start()
    {
        float x = Constant.screenWidth / Screen.width;
        float y = Constant.screenHeight / Screen.height;

        transform.localScale = new Vector3(x, y, 0);
    }
}
