using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理のために必要
using System.Collections; // コルーチンのために必要

public class PlayerHealthAndRewind : MonoBehaviour
{
    public int maxHits = 3; // ゲームオーバーになるまでの最大ヒット数（今回は3回目ヒットでゲームオーバー的な挙動）
    private int currentHits = 0; // 現在のヒット数

    public float rewindDuration = 3f; // 巻き戻す時間（秒）
    public float pauseAfterRewindDuration = 3f; // 巻き戻し後の停止時間（秒）

    private Vector3[] pastPositions; // 過去の位置を記録する配列
    private float[] pastTimes;       // 過去の時間を記録する配列
    private int recordIndex;         // 記録する配列のインデックス
    private int recordCapacity;      // 記録できる最大容量

    public GameObject gameOverPanel; // ゲームオーバー時に表示するUIパネル（今回はゲーム再開ボタンなどを含む想定）

    private Vector3 initialPosition; // プレイヤーの最初の位置を記録

    void Start()
    {
        // 巻き戻しに必要な過去の情報を記録するための容量を計算
        recordCapacity = Mathf.RoundToInt(rewindDuration * 60f) + 5; 
        pastPositions = new Vector3[recordCapacity];
        pastTimes = new float[recordCapacity];
        recordIndex = 0;

        // プレイヤーの最初の位置を記録
        initialPosition = transform.position;

        // ゲームオーバーパネルが設定されている場合は非表示にする
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void FixedUpdate() // 固定フレームレートで物理演算を行うためFixedUpdateを使用
    {
        // ゲームが停止している場合は記録しない
        if (Time.timeScale == 0f) return;

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
            // 同じ障害物に連続して当たらないようにする（必要であれば）
            // 例：障害物をDestroyするか、一時的にColliderを無効にする
            // other.gameObject.SetActive(false); // 障害物を非表示にする例

            currentHits++;
            Debug.Log("Hit! Current Hits: " + currentHits);

            if (currentHits >= maxHits)
            {
                // 3回目以降の衝突処理 (ゲームオーバー or スタート位置に戻す)
                Debug.Log("Hit max hits! Returning to start position.");
                transform.position = initialPosition; // スタート位置に戻す
                currentHits = 0; // ヒット数をリセット（必要であれば）
                
                // ここでゲームオーバーパネルを表示したい場合は表示
                if (gameOverPanel != null)
                {
                    gameOverPanel.SetActive(true); // ゲームオーバーパネルを表示
                    Time.timeScale = 0f; // 時間を停止
                }
                // ゲームオーバーパネルを表示しない場合は、ここでゲームを再開するなどの処理
                // 例: Debug.Log("Player sent back to start.");
            }
            else
            {
                // 1回目、2回目の衝突処理 (巻き戻しと一時停止)
                Rewind();
            }
        }
    }

    void Rewind()
    {
        float targetTime = Time.time - rewindDuration; // 巻き戻したい目標時間

        // 記録された過去の位置の中から、目標時間に最も近い位置を探す
        Vector3 rewindPosition = transform.position; // 見つからなかった場合のデフォルト値
        bool foundRewindPoint = false;

        // 最新の記録から遡る
        for (int i = 0; i < recordCapacity; i++)
        {
            // 正しく循環するインデックスを計算
            int checkIndex = (recordIndex - 1 - i + recordCapacity) % recordCapacity;
            
            // 記録された時間が目標時間以下（それ以前）であれば、その位置に巻き戻す
            if (pastTimes[checkIndex] <= targetTime)
            {
                rewindPosition = pastPositions[checkIndex];
                foundRewindPoint = true;
                break;
            }
            // 配列の最後まで見ても見つからなかった場合（例: ゲーム開始直後で記録がrewindDurationに満たない）
            if (i == recordCapacity - 1) 
            {
                // 念のため、初期位置に戻すなどのフォールバック
                rewindPosition = initialPosition;
                foundRewindPoint = true; // フォールバックでも見つかったとみなす
                Debug.LogWarning("Rewind point not found for " + rewindDuration + "s. Returning to initial position as fallback.");
            }
        }

        transform.position = rewindPosition; // プレイヤーの位置を巻き戻す
        Debug.Log("Rewinding to: " + rewindPosition + " (Hit: " + currentHits + ")");

        // 巻き戻し後のカウントダウンとゲーム再開
        StartCoroutine(PauseAndResumeGame(pauseAfterRewindDuration));
    }

    IEnumerator PauseAndResumeGame(float duration)
    {
        Time.timeScale = 0f; // 時間を停止
        Debug.Log("Game Paused for " + duration + " seconds.");

        float timer = 0f;
        while (timer < duration)
        {
            // UnityのUIでカウントダウンを表示する場合はここに処理を追加
            // 例: UIManager.Instance.UpdateCountdown(duration - timer);
            yield return new WaitForSecondsRealtime(1f); // 実時間で1秒待つ
            timer += 1f;
        }

        Time.timeScale = 1f; // 時間を再開
        Debug.Log("Game Resumed.");
    }

    // ゲームを再開する関数 (ゲームオーバーパネルのボタンなどから呼び出す想定)
    public void RestartGameFromGameOver()
    {
        Time.timeScale = 1f; // 時間を再開
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 現在のシーンをリロード
    }
}