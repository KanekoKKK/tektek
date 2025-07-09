using UnityEngine;

public class SpinAroundPoint : MonoBehaviour
{
    public Transform pivot; //回転軸にするオブジェクト（そのオブジェクトを中心に回る）
    public float rpm = 5f;  //回転速度

    void Update()
    {
        float ang = 360f * rpm / 60f * Time.deltaTime;
        transform.RotateAround(pivot.position, Vector3.up, ang);
    }
}
