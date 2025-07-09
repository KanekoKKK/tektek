using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections; 
using TMPro; // TextMeshProUGUIを使用する場合に必要。Text (Legacy) なら不要。

public class PlayerHealthAndRewind : MonoBehaviour
{
    public int maxHits = 3; // ゲームオーバーになるまでの最大ヒット数（今回は3回目ヒットでゲームオーバー的な挙動）
    private int currentHits = 0; // 現在のヒット数
    private int remainingLives; // 残機として表示する変数

    public float rewindDuration = 3f; // 巻き戻す時間（秒）
    public float pauseAfterRewindDuration = 3f; // 巻き戻し後の停止時間（秒）

    private Vector3[] pastPositions; // 過去の位置を記録する配列
    private float[] pastTimes;       // 過去の時間を記録する配列
    private int recordIndex;         // 記録する配列のインデックス
    private int recordCapacity;      // 記録できる最大容量

    private Vector3 initialPosition; // プレイヤーの最初の位置を記録

    // --- 追加部分 ---
    public TextMeshProUGUI livesText; // TextMeshProUGUIを使用する場合
    // public Text livesText;           // Text (Legacy) を使用する場合
    // --- 追加部分 ---

    void Start()
    {
        recordCapacity = Mathf.RoundToInt(rewindDuration * 60f) + 5; 
        pastPositions = new Vector3[recordCapacity];
        pastTimes = new float[recordCapacity];
        recordIndex = 0;

        initialPosition = transform.position;

        // --- 追加部分 ---
        // ゲーム開始時の残機を設定
        remainingLives = maxHits; 
        UpdateLivesDisplay(); // 残機表示を更新
        // --- 追加部分 ---
    }

    void FixedUpdate() 
    {
        if (Time.timeScale == 0f) return;

        pastPositions[recordIndex] = transform.position;
        pastTimes[recordIndex] = Time.time;
        recordIndex = (recordIndex + 1) % recordCapacity; 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            currentHits++;
            Debug.Log("Hit! Current Hits: " + currentHits);

            // --- 変更部分 ---
            remainingLives--; // 残機を減らす
            UpdateLivesDisplay(); // 残機表示を更新
            // --- 変更部分 ---

            Debug.Log("Returning to start position.");
            transform.position = initialPosition; 
            currentHits = 0; // ヒット数をリセット

            // 残機が0以下になったらゲームオーバーなどの処理を追加できます
            if (remainingLives <= 0)
            {
                Debug.Log("Game Over!");
                // ここでゲームオーバー画面の表示やシーンのリロードなどの処理を行う
                // 例: RestartGameFromGameOver(); // ゲームをリスタートする場合
                Time.timeScale = 0f; // 時間を停止してゲームを一時停止
            }
        }
    }

    IEnumerator PauseAndResumeGame(float duration)
    {
        Time.timeScale = 0f; 
        Debug.Log("Game Paused for " + duration + " seconds.");

        float timer = 0f;
        while (timer < duration)
        {
            yield return new WaitForSecondsRealtime(1f); 
            timer += 1f;
        }

        Time.timeScale = 1f; 
        Debug.Log("Game Resumed.");
    }

    // --- 追加部分 ---
    // 残機表示を更新するメソッド
    void UpdateLivesDisplay()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + Mathf.Max(0, remainingLives); // 0を下回らないように表示
        }
    }
    // --- 追加部分 ---

    public void RestartGameFromGameOver()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}