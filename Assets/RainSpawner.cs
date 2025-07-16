//candyStageで使用

using System.Collections;
using UnityEngine;

public class RainSpawner : MonoBehaviour
{
    [Header("生成対象")]
    public GameObject bigBulletPrefab;

    [Header("発射パターン")]
    public float spawnInterval = 0.7f;      // 何秒おきに撃つか
    public int burstCount = 3;              // 1バーストで何発同時か
    public float burstSpreadX = 8f;         // X 方向にばらけさせる幅
    public float startZ = 15f;              // 出現 Z 座標
    public float yPos   = 0f;               // Y（高さ）

    void Start() => StartCoroutine(SpawnLoop());

    IEnumerator SpawnLoop()
    {
        var wait = new WaitForSeconds(spawnInterval);

        while (true)          // ゲーム終了判定などを挟んでも OK
        {
            FireBurst();
            yield return wait;
        }
    }

    void FireBurst()
    {
        for (int i = 0; i < burstCount; i++)
        {
            float x = Random.Range(-burstSpreadX / 2f, burstSpreadX / 2f);

            Vector3 pos = new Vector3(x, yPos, startZ);
            Instantiate(bigBulletPrefab, pos, Quaternion.identity);
        }
    }
}

