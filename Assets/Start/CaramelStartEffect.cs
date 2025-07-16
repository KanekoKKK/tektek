using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CaramelStartEffect : MonoBehaviour
{
    public Button startButton;
    public RectTransform caramel;
    public float caramelDuration = 1.2f;
    public float fadeDuration = 0.8f;
    public string nextSceneName = "SampleScene";

    private Vector2 startPos;
    private Vector2 endPos;
    private bool isRunning = false;

    void Start()
    {
        startPos = caramel.anchoredPosition;
        endPos = new Vector2(startPos.x, -100); // 画面上にとろ〜り
    }

    public void OnStart()
    {
        Debug.Log("ボタン押された！");
        if (!isRunning)
            StartCoroutine(CaramelEffectCoroutine());
    }

    System.Collections.IEnumerator CaramelEffectCoroutine()
    {
        isRunning = true;

        // カラメルが降りてくるアニメーション
        float t = 0f;
        while (t < caramelDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / caramelDuration);
            caramel.anchoredPosition = Vector2.Lerp(startPos, endPos, lerp);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(nextSceneName);
    }
}
