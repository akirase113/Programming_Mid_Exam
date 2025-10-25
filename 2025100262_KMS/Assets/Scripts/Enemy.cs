using UnityEngine;

// Rigidbody 2D가 필수 컴포넌트임을 명시
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("순찰(Patrol) 설정")]
    public float moveSpeed = 2f;    // 몬스터의 이동 속도
    public float patrolDistance = 3f;   // 시작 위치로부터 좌우로 움직일 거리

    [Header("체력 설정")]
    public int health = 1;    // 몬스터 체력

    // 내부 변수
    private Rigidbody2D rb;
    private Vector2 startPosition;      // 몬스터의 순찰 중심점 (시작 위치)
    private bool isMovingRight = true;  // 현재 오른쪽으로 가는지?
    private bool isAlive = true;        // 몬스터가 살아있는지?

    // 게임 시작 시 1회 호출
    void Start()
    {
        // Rigidbody 2D 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();

        // 몬스터의 시작 위치(순찰의 중심점) 저장
        startPosition = transform.position;

        // 회전 방지 (1단계에서 설정했지만, 코드로도 확인)
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // 매 프레임마다 호출 (방향 전환 로직)
    void Update()
    {
        // 몬스터가 죽었다면 Update 로직을 실행하지 않음
        if (!isAlive) return;

        // 방향 전환 로직
        if (isMovingRight)
        {
            // 만약 오른쪽 한계점(startPosition + patrolDistance)을 넘었다면
            if (transform.position.x >= startPosition.x + patrolDistance)
            {
                isMovingRight = false; // 왼쪽으로 전환
                Flip();
            }
        }
        else // 왼쪽으로 움직일 때
        {
            // 만약 왼쪽 한계점(startPosition - patrolDistance)을 넘었다면
            if (transform.position.x <= startPosition.x - patrolDistance)
            {
                isMovingRight = true; // 오른쪽으로 전환
                Flip();
            }
        }
    }

    // 고정된 프레임마다 호출 (물리 이동 로직)
    void FixedUpdate()
    {
        // 몬스터가 죽었다면 움직임을 멈춤
        if (!isAlive)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // 수평 속도만 0으로
            return;
        }

        // 현재 방향에 맞춰 속도(velocity) 설정
        float moveDirection = isMovingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);
    }

    // 캐릭터 좌우 반전 함수
    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f; // x 스케일 값을 반전 (1 <-> -1)
        transform.localScale = localScale;
    }


    // --- 4단계에서 만든 기존 함수들 ---

    // 데미지를 받는 함수 (PlayerAttack.cs가 호출)
    public void TakeDamage()
    {
        if (!isAlive) return; // 이미 죽었다면 데미지 안 받음

        health--;

        if (health <= 0)
        {
            Die();
        }
    }

    // 죽는 함수
    private void Die()
    {
        isAlive = false; // 살아있지 않음으로 표시 (Update, FixedUpdate가 멈춤)
        Debug.Log("적이 처치되었습니다!");

        // 콜라이더를 비활성화 (클릭 방지)
        GetComponent<Collider2D>().enabled = false;

        // (선택) EnemyClickable 스크립트도 비활성화
        if (GetComponent<EnemyClickable>() != null)
        {
            GetComponent<EnemyClickable>().enabled = false;
        }

        // 물리 효과 완전 정지
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true; // 중력을 포함한 모든 물리 효과를 끔

        // (선택) 여기에 사망 애니메이션 트리거를 추가
        // animator.SetTrigger("Die");

        // (선택) 1초 뒤에 파괴 (애니메이션 시간 기다리기)
        // Destroy(gameObject, 1f);

        // 즉시 파괴
        Destroy(gameObject);
    }
}