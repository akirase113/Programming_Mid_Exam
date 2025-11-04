using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필수
using System.Collections; // 코루틴(시간 지연)을 위해 필수

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
    private bool isDead = false;     // 현재 사망 또는 클리어 상태인지 확인

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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 2. 좌우 입력 받기
        moveInput = Input.GetAxis("Horizontal");

        // 3. 점프 입력 (스페이스바)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // y축 속도 초기화 (일관된 점프 높이)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            
            // y축(위쪽)으로 점프 힘(Impulse)을 가함
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // 4. 애니메이터 파라미터 업데이트
        animator.SetBool("isMoving", moveInput != 0);
        animator.SetBool("isJumping", !isGrounded);

        // 5. 캐릭터 좌우 반전
        Flip();
    }

    // 고정된 시간마다 호출 (물리 처리)
    void FixedUpdate()
    {
        // Rigidbody의 속도(velocity)를 제어하여 좌우 이동
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    // 캐릭터 좌우 반전 함수
    void Flip()
    {
        if ((isFacingRight && moveInput < 0) || (!isFacingRight && moveInput > 0))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // [외부 호출용] 사망 함수
    public void Die()
    {
        // 1. 이미 죽었거나 클리어했다면 중복 실행 방지
        if (isDead) return;

        // 2. 사망 상태로 변경
        isDead = true; 

        // 3. 사망 애니메이션 재생
        animator.SetTrigger("Die");

        // 4. 물리 효과 및 조작 정지
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true; // 중력 및 물리 효과 완전 정지
        this.enabled = false; // PlayerController 스크립트(Update 등) 정지

        // 5. 2초 뒤에 씬을 재시작하는 코루틴 실행
        StartCoroutine(RestartSceneAfterDelay(2.0f));
    }

    // [추가된 함수] Goal.cs가 호출할 클리어 함수
    public void Clear()
    {
        // 1. 이미 죽었거나 클리어했다면 중복 실행 방지
        if (isDead) return;

        // 2. 클리어 상태로 변경
        isDead = true; 

        // 3. 클리어 애니메이션 재생
        // (Animator에 "Clear" Trigger 파라미터가 있어야 합니다)
        animator.SetTrigger("Clear");

        // 4. 물리 효과 및 조작 정지
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        this.enabled = false; // PlayerController 스크립트 정지
    }


    // [추가된 함수] 충돌 감지 함수
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. 충돌한 상대방의 태그가 "Enemy" 또는 "Hazard"인지 확인
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Hazard"))
        {
            Debug.Log(collision.gameObject.name + "과(와) 충돌! 사망합니다.");
            Die(); // Die() 함수 호출
        }
    }

    // [추가된 함수] 2초 뒤에 씬을 재시작하는 코루틴
    private IEnumerator RestartSceneAfterDelay(float delay)
    {
        // 1. 'delay' 시간(초)만큼 기다림
        yield return new WaitForSeconds(delay);

        // 2. 현재 활성화된 씬의 이름을 가져옴 (예: "Stage1")
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        // 3. 현재 씬을 다시 로드(Load)하여 처음부터 시작
        SceneManager.LoadScene(currentSceneName);
    }

} // <--- 클래스(PlayerController)의 마지막 괄호
