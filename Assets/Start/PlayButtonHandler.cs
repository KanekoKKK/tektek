using UnityEngine;
using UnityEngine.UI; // UI要素を扱うために必要
using UnityEngine.SceneManagement; // シーン遷移のために必要 (必要に応じて)

public class PlayButtonHandler : MonoBehaviour
{
    // Unityエディタから設定するPlayボタンへの参照
    public Button playButton;

    // Playボタンが押されたときにロードするシーンの名前
    // この変数は、PlayボタンのOnClickイベントに設定するメソッド内でも使えます
    public string nextSceneName = "GameScene"; // 例: "GameScene"

    void Update()
    {
        // もしEnterキーが押されたら
        if (Input.GetKeyDown(KeyCode.Return)) // KeyCode.Return はエンターキー
        {
            Debug.Log("エンターキーが押されました。Playボタンが押されたものとして処理します。");
            // PlayボタンのOnClickイベントをプログラム的に呼び出す
            if (playButton != null)
            {
                playButton.onClick.Invoke(); // これでボタンがクリックされたのと同じ挙動になります
            }
            else
            {
                Debug.LogWarning("PlayButtonが設定されていません！Inspectorで設定してください。");
            }
        }
    }

    // Playボタンがクリックされたときに実行されるメソッド
    // このメソッドをUnityエディタでPlayボタンのOnClick()イベントに設定します
    public void OnPlayButtonClicked()
    {
        Debug.Log("Playボタンがクリックされました！");
        // ここにシーン遷移などのゲーム開始処理を記述します
        SceneManager.LoadScene(nextSceneName); 
    }
}