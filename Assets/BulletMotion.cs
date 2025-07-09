using UnityEngine;

/// <summary>
/// 発射後：
///   0〜t1 秒     … 等速で進む
///   t1〜t2 秒    … 減速して停止近くまで
///   t2〜無限     … 再加速しながら進む
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class BulletMotion : MonoBehaviour
{
    public float cruiseTime   = 0.6f;   // 等速で進む時間 (t1)
    public float slowTime     = 0.5f;   // 減速フェーズの長さ (t2−t1)
    public float accel        = 10f;    // 再加速用の加速度
    public float lifeTime     = 10f;    // 最大寿命（念のため）

    Rigidbody rb;
    Vector3   dir;           // 進行方向を保持
    float     timer;

    void Start()
    {
        rb  = GetComponent<Rigidbody>();
        dir = rb.velocity.normalized;   // スポナーが与えた初速方向を記憶
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer < cruiseTime)
        {
            // そのまま等速
        }
        else if (timer < cruiseTime + slowTime)
        {
            // 減速：線形補間で徐々に 0 へ
            float t = (timer - cruiseTime) / slowTime;
            float speed = Mathf.Lerp(rb.velocity.magnitude, 0f, t);
            rb.velocity = dir * speed;
        }
        else
        {
            // 再加速：一定加速度で押し出す
            rb.velocity += dir * accel * Time.fixedDeltaTime;
        }

        // 寿命
        if (timer > lifeTime) Destroy(gameObject);
    }

    // 壁と接触したら消える
void OnTriggerEnter(Collider o){
    if(o.CompareTag("Wall")){
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject,0.01f);
    }
}


}
