using UnityEngine;
using TMPro;
using System.Collections;

public class GameClearManager : MonoBehaviour
{
    public GameObject gameClearUI;   // CLEAR パネル
    public TMP_Text text1;           // "GAMECLEAR"
    public TMP_Text text2;           // "Pudding is No.1!!"
    public float fadeDuration = 2f;
    public float displayDuration = 1.5f;

    private CanvasGroup group1;
    private CanvasGroup group2;

    void Start()
    {
        if (gameClearUI == null || text1 == null || text2 == null)
        {
            Debug.LogError("必要なUIが未設定です");
            return;
        }

        gameClearUI.SetActive(false);

        group1 = text1.GetComponent<CanvasGroup>();
        if (group1 == null) group1 = text1.gameObject.AddComponent<CanvasGroup>();
        group1.alpha = 0f;

        group2 = text2.GetComponent<CanvasGroup>();
        if (group2 == null) group2 = text2.gameObject.AddComponent<CanvasGroup>();
        group2.alpha = 0f;

        StartCoroutine(ShowSequence());
    }

    IEnumerator ShowSequence()
{
    yield return new WaitForSecondsRealtime(2f); // ゲーム開始後2秒待つ
    gameClearUI.SetActive(true);
    Time.timeScale = 0f; // 必要なら停止

    // 1つ目（GAMECLEAR）
    yield return StartCoroutine(Fade(group1, text1.transform, true));
    yield return new WaitForSecondsRealtime(displayDuration);
    yield return StartCoroutine(Fade(group1, text1.transform, false));

    // 2つ目（Pudding is No.1!!）→ フェードインだけ！
    yield return StartCoroutine(Fade(group2, text2.transform, true));
    // 表示しっぱなしなのでフェードアウトはしない
}


    IEnumerator Fade(CanvasGroup group, Transform tr, bool fadeIn)
    {
        float timer = 0f;
        float from = fadeIn ? 0f : 1f;
        float to = fadeIn ? 1f : 0f;

        Vector3 startScale = Vector3.one * (fadeIn ? 0.7f : 1f);
        Vector3 endScale = Vector3.one * (fadeIn ? 1f : 0.7f);
        tr.localScale = startScale;

        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            group.alpha = Mathf.Lerp(from, to, t);
            tr.localScale = Vector3.Lerp(startScale, endScale, t);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        group.alpha = to;
        tr.localScale = endScale;
    }
}
