using UnityEngine;

public class LayerSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public float spawnInterval = 1f;
    public float spawnHeight = 1f;
    public float spawnZ = 10f;

    public int columns = 5;           // 横の球の数
    public float startX = -4f;        // 一番左のX座標
    public float spacingX = 2f;       // 球と球の間のX距離

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnRowWithOneSkip();
            timer = 0f;
        }
    }

    void SpawnRowWithOneSkip()
{
    int skipIndex = Random.Range(0, columns);

    for (int i = 0; i < columns; i++)
    {
        if (i == skipIndex) continue;

        float x = startX + i * spacingX;
        Vector3 spawnPos = new Vector3(x, spawnHeight, spawnZ);

        if (ballPrefab != null)
        {
            GameObject ball = Instantiate(ballPrefab, spawnPos, Quaternion.identity);

            // --- 色を固定する処理 ---
            Renderer rend = ball.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = new Color(1f, 0.69f, 0.792f);
            }
        }
        else
        {
            Debug.LogError("Ball Prefab is not assigned in LayerSpawner!");
        }
    }
}

}
