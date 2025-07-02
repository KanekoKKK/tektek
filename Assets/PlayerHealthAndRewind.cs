using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理のために必要

public class PlayerHealthAndRewind : MonoBehaviour
{
    public int maxHits = 3; // ゲームオーバーになるまでの最大ヒット数
    private int currentHits = 0; // 現在のヒット数

    public float rewindDuration = 3f; // 巻き戻す時間（秒）

    private Vector3[] pastPositions; // 過去の位置を記録する配列
    private float[] pastTimes;       // 過去の時間を記録する配列
    private int recordIndex;         // 記録する配列のインデックス
    private int recordCapacity;      // 記録できる最大容量

    public GameObject gameOverPanel; // ゲームオーバー時に表示するUIパネル

    void Start()
    {
        // 巻き戻しに必要な過去の情報を記録するための容量を計算
        // 例: 1秒間に60フレームとして、巻き戻し時間 * フレームレート + α
        recordCapacity = Mathf.RoundToInt(rewindDuration * 60f) + 5; 
        pastPositions = new Vector3[recordCapacity];
        pastTimes = new float[recordCapacity];
        recordIndex = 0;

        // ゲームオーバーパネルが設定されている場合は非表示にする
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void FixedUpdate() // 固定フレームレートで物理演算を行うためFixedUpdateを使用
    {
        // 過去の位置と時間を記録
        pastPositions[recordIndex] = transform.position;
        pastTimes[recordIndex] = Time.time;
        recordIndex = (recordIndex + 1) % recordCapacity; // インデックスを循環させる
    }

    void OnTriggerEnter(Collider other)
    {
        // "Obstacle"タグを持つオブジェクトに当たった場合
        if (other.CompareTag("Obstacle"))
        {
            currentHits++;
            Debug.Log("Hit! Current Hits: " + currentHits);

            if (currentHits >= maxHits)
            {
                // ゲームオーバー処理
                Debug.Log("Game Over!");
                if (gameOverPanel != null)
                {
                    gameOverPanel.SetActive(true); // ゲームオーバーパネルを表示
                }
                Time.timeScale = 0f; // 時間を停止してゲームをフリーズさせる
                // 必要であれば、ここでシーンをロードし直すなどの処理を追加
                // 例: Invoke("RestartGame", 3f); // 3秒後にゲームを再開する場合
            }
            else
            {
                // 巻き戻し処理
                Rewind();
            }
        }
    }

    void Rewind()
    {
        float targetTime = Time.time - rewindDuration; // 巻き戻したい目標時間

        // 記録された過去の位置の中から、目標時間に最も近い位置を探す
        Vector3 rewindPosition = transform.position; // 見つからなかった場合のデフォルト値
        for (int i = 0; i < recordCapacity; i++)
        {
            int checkIndex = (recordIndex - 1 - i + recordCapacity) % recordCapacity; // 最新の記録から遡る
            if (pastTimes[checkIndex] <= targetTime)
            {
                rewindPosition = pastPositions[checkIndex];
                break;
            }
        }
        transform.position = rewindPosition; // プレイヤーの位置を巻き戻す
        Debug.Log("Rewinding to: " + rewindPosition);
    }

    // ゲームを再開する例（ゲームオーバー後に使う場合）
    // void RestartGame()
    // {
    //     Time.timeScale = 1f; // 時間を再開
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 現在のシーンをリロード
    // }
}