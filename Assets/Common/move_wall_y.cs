using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_wall_y : MonoBehaviour
{
    Transform myTransform;
    Vector3 position_start;
    Vector3 position_now;

    private int flag;

    private float count;
   

    Color32 Color1 = new Color32(255,0,0,1); // Color1 の名前で色を定義する.

    Color32 Color2 = new Color32(0,255,0,1); // Color2 の名前で色を定義する.

    // Start is called before the first frame update
    void Start()
    {
        myTransform = this.transform;
        position_start = myTransform.position;
        position_now = position_start;
        GetComponent<Renderer>().material.color = Color1; // オブジェクトの色を Color1 にする
        count = 0.0f;
        flag = 0; // 初期状態では flag を 0 に設定
    }

    // Update is called once per frame
    void Update()
    {
        if (flag == 0)
        {
            position_now.y += 0.05f;
            if (position_now.y - position_start.y > 1)
            {
                flag = 1;
                GetComponent<Renderer>().material.color = Color2; // オブジェクトの色を Color2 にする
                while (count < 5.0f)
                {
                    // 5秒間待機する
                    count += Time.deltaTime;
                }
                count = 0.0f; // カウントをリセット
                
            }
            myTransform.position = position_now;
        }

        if (flag == 1)
        {
            position_now.y -= 0.05f;
            if (position_start.y - position_now.y > 1)
            {
                flag = 0;
                GetComponent<Renderer>().material.color = Color1; // オブジェクトの色を Color1 にする
            }
            myTransform.position = position_now;
        }
        
    }
}
