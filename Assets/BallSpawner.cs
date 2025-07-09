using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;             // プレハブ（球）
    public float spawnInterval = 0.5f;        // 生成間隔
    public Vector2 spawnRangeX = new Vector2(-5f, 5f); // X方向にランダム生成
    public float spawnHeight = 1f;            // Y座標
    public float spawnZ = 10f;                // Z座標（奥の位置）

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnBall();
            timer = 0f;
        }
    }

   void SpawnBall()
{
    float x = Random.Range(spawnRangeX.x, spawnRangeX.y);
    Vector3 spawnPos = new Vector3(x, spawnHeight, spawnZ);

    if (ballPrefab != null)
    {
        GameObject ball = Instantiate(ballPrefab, spawnPos, Quaternion.identity);
        
        // ランダム速度（例：2.0〜5.0の範囲）
        float randomSpeed = Random.Range(2.0f, 5.0f);

        AutoMoveAndDestroy amd = ball.GetComponent<AutoMoveAndDestroy>();
        if (amd != null)
        {
            amd.SetSpeed(randomSpeed);
        }

        // ランダムな色を設定
        Renderer rend = ball.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = new Color(
                Random.Range(0.5f, 1f),  // R
                Random.Range(0.5f, 1f),  // G
                Random.Range(0.5f, 1f)   // B
            );
        }
    }
    else
    {
        Debug.LogError("Ball Prefab is not assigned in BallSpawner!");
    }
}


}
