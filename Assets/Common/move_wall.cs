using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_wall : MonoBehaviour
{
    Transform myTransform;
    Vector3 position_start;
    Vector3 position_now;

    private int flag = 0;

    Color32 Color1 = new Color32(255,0,0,1); // Color1 の名前で色を定義する.

    Color32 Color2 = new Color32(0,255,0,1); // Color2 の名前で色を定義する.

    // Start is called before the first frame update
    void Start()
    {
        myTransform = this.transform;
        position_start = myTransform.position;
        position_now = position_start;
        // GetComponent<Renderer>().material.color = Color1; // オブジェクトの色を Color1 にする
    }

    // Update is called once per frame
    void Update()
    {
        if (flag == 0)
        {
            position_now.x += 0.05f;
            if (position_now.x - position_start.x > 5)
            {
                flag = 1;
                // GetComponent<Renderer>().material.color = Color2; // オブジェクトの色を Color2 にする
            }
            myTransform.position = position_now;
        }

        if (flag == 1)
        {
            position_now.x -= 0.05f;
            if (position_start.x - position_now.x > 5)
            {
                flag = 0;
                // GetComponent<Renderer>().material.color = Color1; // オブジェクトの色を Color1 にする
            }
            myTransform.position = position_now;
        }
        
    }
}
