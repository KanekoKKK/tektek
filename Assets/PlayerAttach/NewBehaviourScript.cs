using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float moveSpeed = 5.0f; // 通常の移動速度（秒速）
    public float dashSpeedMultiplier = 2.0f; // ダッシュ時の速度倍率
    public float jumpForce = 7.0f; // ジャンプ力
    public float rotationSpeed = 10.0f; // 回転速度

    private bool isGrounded; // 地面にいるかどうかのフラグ
    private Rigidbody rb; // Rigidbodyコンポーネンスへの参照

    private Animator animator; // アニメーション

    // Start is called before the first frame-update
    void Start()
    {
        Application.targetFrameRate = 60; // ターゲットフレームレートを60FPSに設定
        // Rigidbodyコンポーネントを取得
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on this GameObject. Please add a Rigidbody to the player.");
        }
        // Animatorコンポーネントを取得
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        if (animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject or its children.");
        }
    }

    // Updateはキー入力の検出に適しています
    void Update()
    {
        // --- ジャンプ機能 ---
        // Spaceキーが押され、かつ地面にいる場合
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // 上方向に力を加える (ForceMode.Impulse は瞬間的な力を加える)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // ジャンプしたので地面から離れた状態にする
        }

    }

    // FixedUpdateは物理演算の更新に適しています
    void FixedUpdate()
    {
        // --- 移動処理 ---
        float currentMoveSpeed = moveSpeed;

        // ダッシュ機能: Shiftキーを押している間は速度を上げる
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentMoveSpeed *= dashSpeedMultiplier;
        }

        // 入力に基づいて移動方向を計算 (ワールド座標)
        Vector3 moveInput = Vector3.zero;

        // "up" キー（ワールドZ軸のプラス方向へ移動）
        if (Input.GetKey("up"))
        {
            moveInput += Vector3.forward;
        }
        // "down" キー（ワールドZ軸のマイナス方向へ移動）
        if (Input.GetKey("down"))
        {
            moveInput += Vector3.back;
        }
        // "right" キー（ワールドX軸のプラス方向へ移動）
        if (Input.GetKey("right"))
        {
            moveInput += Vector3.right;
        }
        // "left" キー（ワールドX軸のマイナス方向へ移動）
        if (Input.GetKey("left"))
        {
            moveInput += Vector3.left;
        }

        // 斜め移動の速度を均一にするために正規化
        // （例: "up"と"right"が同時に押されても速度が速くなりすぎないようにする）
        if (moveInput.magnitude > 1)
        {
            moveInput.Normalize();
        }

        // Rigidbody の速度を設定
        // Y軸の速度（ジャンプなどによるもの）は保持する
        Vector3 newVelocity = moveInput * currentMoveSpeed;
        newVelocity.y = rb.velocity.y; // 現在のY軸速度（重力やジャンプ）を維持
        rb.velocity = newVelocity;

        // --- 向きの変更 ---
        // moveInput がゼロでない場合（つまり移動入力がある場合）のみ向きを変える
        if (moveInput != Vector3.zero)
        {
            // 移動方向へ向かうQuaternionを作成
            // Y軸固定
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveInput.x, 0, moveInput.z));

            // 現在の回転から目標の回転へ滑らかに補間
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }       

        // --- アニメーション ---
        if (animator != null)
        {
            // 移動しているかどうかを判定
            // moveInputのmagnitudeが0より大きい場合、移動していると判断
            // Rigidbodyを使っているので、rb.velocityの水平方向のmagnitudeを使ってもよい
            bool isWalking = moveInput.magnitude > 0.01f; // 小さな値でしきい値を設ける
            // AnimatorのParametersを更新
            animator.SetBool("isWalking", isWalking);
        }
    }

    //--- 地面との接触判定 ---
    void OnCollisionEnter(Collision collision)
    {
        // プレイヤーが地面（またはジャンプ可能な表面）に触れた場合
        // 必ず「Ground」タグを地面となるオブジェクトに設定してください
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // 地面から離れた場合の判定（必須ではありませんが、より正確な接地判定のために使用できます）
    void OnCollisionExit(Collision collision)
    {
        // ここでisGroundedをfalseにするロジックを追加することも可能ですが、
        // ジャンプ時にisGroundedをfalseにする方が一般的で、より堅牢な実装になります。
        // （例: 傾斜で一瞬接地を失った場合にジャンプできなくなるのを避けるため）
    }
}