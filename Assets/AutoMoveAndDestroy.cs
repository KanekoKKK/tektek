using UnityEngine;

public class AutoMoveAndDestroy : MonoBehaviour
{
    public float speed = 3f;
    public float destroyZ = -20f;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (transform.position.z < destroyZ)
        {
            Destroy(gameObject);
        }
    }
}
