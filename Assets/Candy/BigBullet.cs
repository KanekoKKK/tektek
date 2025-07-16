using System.Collections;
using UnityEngine;

/// <summary>
/// Z = +15 あたりでスポーン → Z 負方向へ移動 → ランダム Z で停止 → 6 分裂
/// </summary>
public class BigBullet: MonoBehaviour
{
    [Header("落下設定")]
    public float fallSpeed = 6f;          // Z− 方向の速度
    public float minStopZ = 2f;           // 停止 Z（カメラ手前寄り）
    public float maxStopZ = 8f;           // 停止 Z（カメラ奥寄り）

    [Header("分裂設定")]
    public GameObject smallBulletPrefab;
    public int splitCount = 6;
    public float smallSpeed = 8f;
    public float smallScale = 0.5f;
    public float splitDelay = 0.1f;

    float _stopZ;
    bool _splitting;

    void Start()
    {
        _stopZ = Random.Range(minStopZ, maxStopZ);
    }

    void Update()
    {
        if (_splitting) return;

        // Z− 方向へ移動（XZ 平面上）
        transform.Translate(Vector3.back * fallSpeed * Time.deltaTime, Space.World);

        // 停止判定
        if (transform.position.z <= _stopZ)
        {
            StartCoroutine(Split());
            _splitting = true;
        }
    }

    IEnumerator Split()
    {
        yield return new WaitForSeconds(splitDelay);

        float step = 360f / splitCount;

        for (int i = 0; i < splitCount; i++)
        {
            float rad = step * i * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)).normalized;

            GameObject b = Instantiate(smallBulletPrefab, transform.position, Quaternion.identity);
            b.transform.localScale *= smallScale;

            // Rigidbody がある場合
            if (b.TryGetComponent(out Rigidbody rb))
                rb.velocity = dir * smallSpeed;
            else if (b.TryGetComponent(out SmallBullet sb))
                sb.Set(dir, smallSpeed);
        }

        Destroy(gameObject);   // 親弾は不要になったら削除
    }
}
