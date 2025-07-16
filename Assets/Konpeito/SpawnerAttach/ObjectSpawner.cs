using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public ObjectPoolManager objectPoolManager; // オブジェクトプールマネージャーへの参照
    public float spawnInterval = 2.0f;          // オブジェクト生成を試みる間隔
    public float spawnProbability = 0.7f;       // オブジェクトをスポーンさせる確率 (0.0 ～ 1.0)
    public float spawnXPosition = 15.0f;        // オブジェクトが出現するX座標
    public float objectYPosition = 0.0f;        // オブジェクトのY座標
    public float objectZPosition = 0.0f;        // オブジェクトのZ座標

    void Start()
    {
        // ObjectPoolManagerへの参照が設定されているか確認
        if (objectPoolManager == null)
        {
            // シーン内から自動で検索 (推奨はInspectorからの設定)
            objectPoolManager = FindObjectOfType<ObjectPoolManager>();
            if (objectPoolManager == null)
            {
                Debug.LogError("ObjectPoolManager not found in the scene! Please assign it in the Inspector.");
                this.enabled = false; // スクリプトを無効化してエラーの連鎖を防ぐ
                return;
            }
        }
        StartCoroutine(SpawnObjectsCoroutine());
    }

    IEnumerator SpawnObjectsCoroutine()
    {
        while (true)
        {
            // 確率的にスポーンさせるかどうかを決定
            if (Random.value < spawnProbability) // Random.value は 0.0～1.0 の値を返す
            {
                // プールからオブジェクトを取得
                GameObject objToSpawn = objectPoolManager.GetPooledObject();
                
                // 取得したオブジェクトの位置を設定
                objToSpawn.transform.position = new Vector3(spawnXPosition, objectYPosition, objectZPosition);
                
                // タグを設定 (プレハブにObstacleタグが設定済みなら不要)
                objToSpawn.tag = "Obstacle";

                // MovingObjectスクリプトにリセット位置を伝える
                MovingObject movingScript = objToSpawn.GetComponent<MovingObject>();
                if (movingScript != null)
                {
                    // オブジェクトが画面外に出たときにプールに戻すように設定
                    movingScript.objectPoolManager = objectPoolManager; // MovingObjectにプールマネージャーを渡す
                    movingScript.resetXPosition = -10.5f; // 例：画面左端でプールに戻すX座標
                    movingScript.moveSpeed = 5.0f; // 必要に応じて速度をリセット
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}