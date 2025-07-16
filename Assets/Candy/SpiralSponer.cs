// SpiralSponer.cs  ※角度を回しながら等速で発射
using UnityEngine;

public class SpiralSponer : MonoBehaviour
{
    [Header("弾設定")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    public float fireInterval = 0.05f;
    public float angleStep = 10f;

    float currentAngle;
    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fireInterval)
        {
            FireBullet();
            timer = 0f;
            currentAngle += angleStep;
        }
    }

    void FireBullet()
    {
        float rad = currentAngle * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));   // XZ 平面へ

        Vector3 spawnPos = transform.position;

        GameObject b  = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        Rigidbody rb = b.GetComponent<Rigidbody>();
        rb.velocity  = dir.normalized * bulletSpeed;                     
    }
}
