using UnityEngine;

public class SpiralSponer : MonoBehaviour
{
    [Header("弾設定")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    public float fireInterval = 0.05f;
    public float angleStep = 10f;

    float currentAngle = 0f;
    float currentSpeed = 0f;
    float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fireInterval)
        {
            FireBullet();
            timer = 0f;
            currentAngle += angleStep;
            currentSpeed += 1f;    //加速
        }
    }

void FireBullet()
{
    float rad = currentAngle * Mathf.Deg2Rad;

   
    Vector3 dir = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));  //xz平面上を進む（y = 0f）

  
    Vector3 spawnPos = transform.position;

    GameObject b  = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
    Rigidbody rb = b.GetComponent<Rigidbody>();

    rb.velocity = new Vector3(dir.x, 0f, dir.z) * bulletSpeed;


}
}
