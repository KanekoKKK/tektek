using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections; 
using TMPro; // TextMeshProUGUIを使用する場合に必要。Text (Legacy) なら不要。
using UnityEngine.UI;

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

    public TextMeshProUGUI livesText; // TextMeshProUGUIを使用する場合
    // public Text livesText;         // Text (Legacy) を使用する場合

    public Image[] puddingImages;

    // --- 無敵モード関連の追加 ---
    [Header("無敵モード設定")]
    [SerializeField]
    private KeyCode[] secretCommandKeys = { KeyCode.U, KeyCode.N, KeyCode.I, KeyCode.T, KeyCode.Y }; // 隠しコマンドのキー配列
    private int currentCommandIndex = 0; // 現在入力されているコマンドのインデックス
    private float commandResetTime = 1.5f; // コマンド入力のリセット時間（秒）
    private float lastInputTime; // 最後のコマンド入力時間

    [SerializeField]
    private float invincibilityDuration = 10f; // 無敵モードの持続時間（秒）
    private bool isInvincible = false; // 無敵モードのオン/オフを管理するフラグ
    private Coroutine invincibilityCoroutine; // 無敵コルーチンを保持
    // --- 無敵モード関連の追加ここまで ---

    void Start()
    {
        recordCapacity = Mathf.RoundToInt(rewindDuration * 60f) + 5; 
        pastPositions = new Vector3[recordCapacity];
        pastTimes = new float[recordCapacity];
        recordIndex = 0;

        initialPosition = transform.position;

        remainingLives = maxHits; 
        UpdateLivesDisplay(); // 残機表示を更新

        // 無敵コマンドのデバッグ表示
        Debug.Log("無敵モードの隠しコマンド: " + GetCommandString());
    }

    void FixedUpdate() 
    {
        if (Time.timeScale == 0f) return;

        pastPositions[recordIndex] = transform.position;
        pastTimes[recordIndex] = Time.time;
        recordIndex = (recordIndex + 1) % recordCapacity; 
    }

    void Update()
    {
        // 毎フレーム隠しコマンドの入力をチェック
        CheckSecretCommandInput();
    }

    void OnTriggerEnter(Collider other)
    {
        // 無敵モード中はObstacleタグのオブジェクトに触れてもダメージを受けない
        if (other.CompareTag("Obstacle"))
        {
            if (isInvincible) // 無敵モード中なら何もしない
            {
                Debug.Log("無敵モード中なので障害物ダメージを無効化！");
                return;
            }

            // 無敵モードでなければ通常通りダメージ処理
            currentHits++;
            Debug.Log("Hit! Current Hits: " + currentHits);

            remainingLives--; // 残機を減らす
            UpdateLivesDisplay(); // 残機表示を更新

            // プリン画像の表示を更新
            if (remainingLives >= 0 && remainingLives < puddingImages.Length)
            {
                puddingImages[remainingLives].enabled = false;
            } else if (remainingLives < 0 && puddingImages.Length > 0)
            {
                // 残機が0を下回った場合、最後のプリン画像を非表示にするなど
                puddingImages[0].enabled = false; 
            }


            Debug.Log("Returning to start position.");
            transform.position = initialPosition; 
            currentHits = 0; // ヒット数をリセット

            if (remainingLives <= 0)
            {
                Debug.Log("Game Over!");
                Time.timeScale = 0f; // 時間を停止してゲームを一時停止
                // ここでゲームオーバー画面の表示やシーンのリロードなどの処理を行う
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

    void UpdateLivesDisplay()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + Mathf.Max(0, remainingLives); // 0を下回らないように表示
        }
    }

    public void RestartGameFromGameOver()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    // --- 無敵モード関連の追加メソッド ---
    private void CheckSecretCommandInput()
    {
        // 入力リセットタイマー
        if (Time.time - lastInputTime > commandResetTime)
        {
            currentCommandIndex = 0;
        }

        // コマンドの各キーをチェック
        if (currentCommandIndex < secretCommandKeys.Length)
        {
            if (Input.GetKeyDown(secretCommandKeys[currentCommandIndex]))
            {
                currentCommandIndex++;
                lastInputTime = Time.time; // 最後の入力時間を更新
                Debug.Log($"コマンド入力: {secretCommandKeys[currentCommandIndex - 1].ToString()} ({currentCommandIndex}/{secretCommandKeys.Length})");

                if (currentCommandIndex >= secretCommandKeys.Length)
                {
                    // コマンドが全て入力された！
                    Debug.Log("<color=green>隠しコマンド成功！無敵モード発動！</color>");
                    ActivateInvincibility();
                    currentCommandIndex = 0; // コマンドをリセット
                }
            }
        }
    }

    private void ActivateInvincibility()
    {
        if (invincibilityCoroutine != null)
        {
            StopCoroutine(invincibilityCoroutine); // 既に無敵モードなら一度停止
        }
        invincibilityCoroutine = StartCoroutine(InvincibilityRoutine());
    }

    IEnumerator InvincibilityRoutine()
    {
        isInvincible = true; // 無敵フラグをON
        Debug.Log($"無敵モードON！残り{invincibilityDuration}秒");

        // ここでプレイヤーに無敵エフェクト（点滅、透明化など）を適用する処理を入れる
        // 例: GetComponent<Renderer>().material.color = Color.yellow; // 例として色を変える
        // PlayerController.Instance.SetInvincibleEffect(true); // もしPlayerControllerがある場合

        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false; // 無敵フラグをOFF
        Debug.Log("<color=red>無敵モードOFF！</color>");

        // ここでプレイヤーの無敵エフェクトを解除する処理を入れる
        // 例: GetComponent<Renderer>().material.color = Color.white; // 例として色を戻す
        // PlayerController.Instance.SetInvincibleEffect(false); // もしPlayerControllerがある場合

        invincibilityCoroutine = null; // コルーチンが終了したのでnullに
    }

    // コマンド文字列をデバッグ表示用
    private string GetCommandString()
    {
        string command = "";
        foreach (KeyCode key in secretCommandKeys)
        {
            command += key.ToString() + " ";
        }
        return command.Trim();
    }
    // --- 無敵モード関連の追加メソッドここまで ---
}