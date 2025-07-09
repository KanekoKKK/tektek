using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float fallSpeed = 5.0f;
    public float resetYPosition = -5.0f;

    [HideInInspector]
    public ObjectPoolManager objectPoolManager;

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        if (transform.position.y < resetYPosition)
        {
            if (objectPoolManager != null)
            {
                objectPoolManager.ReturnPooledObject(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
