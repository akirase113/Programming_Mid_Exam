using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("이동 및 점프 설정")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("지면 체크 설정")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    // 내부 변수
    private Rigidbody2D rb;
    private Animator animator;
    private float moveInput;
    private bool isGrounded;
    private bool isFacingRight = true;
    private bool isDead = false; // 사망 또는 클리어 상태인지 확인

    // 게임 시작 시 1회 호출
    void Start()
    {
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
        // 지면 감지
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 좌우 입력 받기
        moveInput = Input.GetAxis("Horizontal");

        // 점프 입력
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // 애니메이터 파라미터 업데이트
        animator.SetBool("isMoving", moveInput != 0);
        animator.SetBool("isJumping", !isGrounded);

        // 캐릭터 좌우 반전
        Flip();
    }

    // 고정된 시간마다 호출 (물리 처리)
    void FixedUpdate()
    {
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
        if (isDead) return; // 중복 실행 방지
        isDead = true;

        // GameUIManager를 찾아 GameOver 이미지 표시 요청
        GameUIManager uiManager = FindObjectOfType<GameUIManager>();
        if (uiManager != null)
        {
            uiManager.ShowGameOver();
        }

        animator.SetTrigger("Die");
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        this.enabled = false;

        // 2초 뒤에 씬 재시작
        StartCoroutine(RestartSceneAfterDelay(2.0f));
    }

    // [추가됨!] Goal.cs가 호출할 클리어 함수
    public void Clear()
    {
        if (isDead) return; // 이미 죽었다면 클리어 불가
        isDead = true; // 클리어 상태 (조작 방지)

        // 클리어 애니메이션 재생
        animator.SetTrigger("Clear");

        // 물리 효과 및 조작 정지
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        this.enabled = false; // 스크립트 정지
    }

    // 충돌 감지 (가시, 적)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Hazard"))
        {
            Die();
        }
    }

    // 씬 재시작 코루틴
    private IEnumerator RestartSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}