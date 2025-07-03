using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理のために必要

public class GoalTrigger : MonoBehaviour
{
    public string nextSceneName; // インスペクターから次のシーン名を指定できるようにする

    private void OnTriggerEnter(Collider other)
    {
        // ゴールに触れたのがプレイヤーかどうかをタグで判断
        // プレイヤーオブジェクトに "Player" タグを設定しておくことを推奨
        if (other.CompareTag("Player"))
        {
            Debug.Log("ゴール！次のシーンへ移動します: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}