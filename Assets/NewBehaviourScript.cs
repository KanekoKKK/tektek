using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float moveSpeed = 0.1f; // 通常の移動速度
    public float dashSpeedMultiplier = 2.0f; // ダッシュ時の速度倍率
    public float jumpForce = 5.0f; // ジャンプ力

    private bool isGrounded; // 地面にいるかどうかのフラグ
    private Rigidbody rb; // Rigidbodyコンポーネンスへの参照

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60; // Set the target frame rate to 60 FPS
        rb = GetComponent<Rigidbody>(); // Rigidbodyコンポーネントを取得
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on this GameObject. Please add a Rigidbody to the player.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // --- 移動処理 ---
        float currentMoveSpeed = moveSpeed;

        // ダッシュ機能: Shiftキーを押している間は速度を上げる
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentMoveSpeed *= dashSpeedMultiplier;
        }

        if (Input.GetKey("up"))
        {
            transform.position += transform.forward * currentMoveSpeed;
        }
        if (Input.GetKey("down"))
        {
            transform.position -= transform.forward * currentMoveSpeed;
        }
        if (Input.GetKey("right"))
        {
            transform.Rotate(0f, 3.0f, 0f);
        }
        if (Input.GetKey("left"))
        {
            transform.Rotate(0f, -3.0f, 0f);
        }

        // --- ジャンプ機能 ---
        // Spaceキーが押され、かつ地面にいる場合
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // 上方向に力を加える
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // ジャンプしたので地面から離れた状態にする
        }
    }

    // 地面との接触判定
    void OnCollisionEnter(Collision collision)
    {
        // プレイヤーが地面（またはジャンプ可能な表面）に触れた場合
        // ここでは"Ground"タグを例にしていますが、適切なタグを設定してください
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // 地面から離れた場合の判定（任意だが推奨）
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 必要に応じて、ここでisGroundedをfalseにするロジックを追加することもできます。
            // ただし、ジャンプ時に即座にfalseにする方が望ましい場合が多いです。
            // 複数のオブジェクトに触れている可能性があるため、OnCollisionEnterでfalseにするのは注意が必要です。
        }
    }
}