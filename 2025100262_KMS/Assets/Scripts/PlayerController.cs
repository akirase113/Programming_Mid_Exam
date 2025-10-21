using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 및 점프 설정")]
    public float moveSpeed = 5f;     // 이동 속도
    public float jumpForce = 10f;    // 점프 힘

    [Header("지면 체크 설정")]
    public Transform groundCheck;    // 발밑 지면 감지용 오브젝트
    public float groundCheckRadius = 0.2f; // 지면 감지 범위 (원형)
    public LayerMask groundLayer;    // 'Ground'로 설정된 레이어

    // 내부에서 사용할 변수
    private Rigidbody2D rb;          // 물리 컴포넌트
    private Animator animator;       // 애니메이터 컴포넌트
    private float moveInput;         // 좌우 입력 값
    private bool isGrounded;         // 땅에 닿았는지 여부
    private bool isFacingRight = true; // 현재 오른쪽을 보는지 여부

    // 게임 시작 시 1회 호출
    void Start()
    {
        // Player 오브젝트에 붙어있는 컴포넌트들을 가져와서 변수에 저장
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck 오브젝트가 설정되지 않았습니다!");
        }
    }

    // 매 프레임마다 호출 (입력 감지)
    void Update()
    {
        // 1. 지면 감지
        // groundCheck의 위치에서 groundCheckRadius 반경의 원을 그려
        // groundLayer와 겹치는지 확인
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 2. 좌우 입력 받기 (A, D, <-, -> 키)
        moveInput = Input.GetAxis("Horizontal"); // -1 (왼쪽) ~ +1 (오른쪽)

        // 3. 점프 입력 (스페이스바)
        // 만약 'Jump' 버튼을 눌렀고 (GetButtonDown) 땅에 닿아있다면 (isGrounded)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // y축 속도를 0으로 초기화 (더블 점프 방지 및 일관된 점프 높이)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            
            // y축(위쪽)으로 점프 힘(jumpForce)을 순간적으로 가함 (Impulse)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // 4. 애니메이터 파라미터 업데이트
        animator.SetBool("isMoving", moveInput != 0); // moveInput이 0이 아니면 isMoving = true
        animator.SetBool("isJumping", !isGrounded); // 땅에 닿지 않았다면 isJumping = true

        // 5. 캐릭터 좌우 반전
        Flip();
    }

    // 고정된 시간마다 호출 (물리 처리)
    void FixedUpdate()
    {
        // Rigidbody의 속도(velocity)를 제어하여 좌우 이동 구현
        // y축 속도(rb.velocity.y)는 점프나 중력을 위해 그대로 유지해야 함
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    // 캐릭터 좌우 반전 함수
    void Flip()
    {
        // 왼쪽으로 가려는데 오른쪽을 보고 있거나 (moveInput < 0 && isFacingRight)
        // 오른쪽으로 가려는데 왼쪽을 보고 있다면 (!isFacingRight && moveInput > 0)
        if ((isFacingRight && moveInput < 0) || (!isFacingRight && moveInput > 0))
        {
            isFacingRight = !isFacingRight; // 보는 방향 플래그 뒤집기

            // 오브젝트의 스케일(Scale)의 x 값을 -1 곱하여 좌우를 뒤집음
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // [외부 호출용] 사망 함수 (Obstacle.cs 등에서 호출)
    public void Die()
    {
        animator.SetTrigger("Die");
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true; // 물리 효과 정지
        this.enabled = false; // 이 스크립트의 Update/FixedUpdate 정지
    }

    // [외부 호출용] 클리어 함수 (Goal.cs 등에서 호출)
    public void Clear()
    {
        animator.SetTrigger("Clear");
        rb.linearVelocity = Vector2.zero;
        this.enabled = false;
    }
}