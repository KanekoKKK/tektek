using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float moveSpeed = 5.0f;          // オブジェクトの移動速度
    public float resetXPosition = -15.0f;   // このX座標に到達したらプールに戻す
    
    [HideInInspector] // Inspectorには表示しない（ObjectSpawnerから設定されるため）
    public ObjectPoolManager objectPoolManager; // ObjectPoolManagerへの参照

    void Update()
    {
        // オブジェクトを左に移動
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // 特定のX座標に到達したら、プールに戻す
        if (transform.position.x < resetXPosition)
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