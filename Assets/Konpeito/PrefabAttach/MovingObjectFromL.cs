using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectFromL : MonoBehaviour
{
    public float moveSpeed = 5.0f;          // オブジェクトの移動速度
    public float resetXPosition = 15.0f;   // このX座標に到達したらプールに戻す（画面右端など）
    
    [HideInInspector] // Inspectorには表示しない（ObjectSpawnerから設定されるため）
    public ObjectPoolManager objectPoolManager; // ObjectPoolManagerへの参照

    void Update()
    {
        // オブジェクトを右に移動
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // 特定のX座標（画面の右端など）に到達したら、プールに戻す
        if (transform.position.x > resetXPosition) // ★条件を '<' から '>' に変更★
        {
            if (objectPoolManager != null)
            {
                objectPoolManager.ReturnPooledObject(this.gameObject);
            }
            else
            {
                // 万が一プールマネージャーが設定されていない場合のフォールバック
                Destroy(gameObject);
                Debug.LogWarning("MovingObject tried to return to pool but ObjectPoolManager was null. Destroyed instead.");
            }
        }
    }
}