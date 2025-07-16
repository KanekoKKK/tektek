using UnityEngine;

public class SmallBullet: MonoBehaviour
{
    Vector3 _dir;
    float _speed;
    [SerializeField] float life = 5f;

    public void Set(Vector3 dir, float speed)
    {
        _dir = dir;
        _speed = speed;
    }

    void Update()
    {
        transform.Translate(_dir * _speed * Time.deltaTime, Space.World);
        if ((life -= Time.deltaTime) <= 0f) Destroy(gameObject);
    }
}
