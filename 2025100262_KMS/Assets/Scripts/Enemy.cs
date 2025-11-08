using UnityEngine;

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
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // [이동 로직] 매 프레임마다 호출 (방향 전환 로직)
    void Update()
    {
        if (!isAlive) return; // 죽었으면 움직이지 않음

        // 방향 전환 로직
        if (isMovingRight)
        {
            if (transform.position.x >= startPosition.x + patrolDistance)
            {
                isMovingRight = false;
                Flip();
            }
        }
        else // 왼쪽으로 움직일 때
        {
            if (transform.position.x <= startPosition.x - patrolDistance)
            {
                isMovingRight = true;
                Flip();
            }
        }
    }

    // [이동 로직] 고정된 프레임마다 호출 (물리 이동 로직)
    void FixedUpdate()
    {
        if (!isAlive)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        float moveDirection = isMovingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);
    }

    // [이동 로직] 캐릭터 좌우 반전 함수
    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    // 데미지를 받는 함수 (PlayerAttack.cs가 호출)
    public void TakeDamage()
    {
        if (!isAlive) return;

        health--;

        if (health <= 0)
        {
            Die();
        }
    }

    // 죽는 함수 (GameUIManager에 보고하는 기능 포함)
    private void Die()
    {
        if (!isAlive) return;

        isAlive = false;
        Debug.Log("적이 처치되었습니다!");

        // 1. [수정됨] GameUIManager에 보고
        if (GameUIManager.instance != null)
        {
            GameUIManager.instance.EnemyDefeated();
        }

        // 2. 콜라이더 및 물리 정지
        GetComponent<Collider2D>().enabled = false;
        if (GetComponent<EnemyClickable>() != null)
        {
            GetComponent<EnemyClickable>().enabled = false;
        }
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;

        // 3. 오브젝트 파괴
        Destroy(gameObject);
    }
}