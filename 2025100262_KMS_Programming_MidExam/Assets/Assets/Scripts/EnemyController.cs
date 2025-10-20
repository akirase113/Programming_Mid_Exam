using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;          // 적의 이동 속도
    public string direction = "left";     // 적의 이동 방향 (left, right)
    public float range = 2.0f;          // 움직이는 범위
    private Vector3 defPos;             // 시작 위치

    // Rigidbody2D 컴포넌트를 캐싱할 변수
    private Rigidbody2D rbody;

    private void Awake()
    {
        // GetComponent는 성능을 위해 Awake나 Start에서 한 번만 호출합니다.
        rbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (direction == "right")
        {
            transform.localScale = new Vector2(-1, 1); // 오른쪽을 바라보도록 설정
        }
        else
        {
            transform.localScale = new Vector2(1, 1); // 왼쪽을 바라보도록 설정
        }
        defPos = transform.position; // 시작 위치 저장
    }

    private void Update()
    {
        if (range > 0.0f)
        {
            // 현재 방향이 "left"일 때만 왼쪽 끝 체크를 하도록 수정 (떨림 방지)
            if (transform.position.x < defPos.x - (range / 2) && direction == "left")
            {
                Flip(); // 중복 코드를 Flip() 함수로 대체
            }
            // 현재 방향이 "right"일 때만 오른쪽 끝 체크를 하도록 수정 (떨림 방지)
            else if (transform.position.x > defPos.x + (range / 2) && direction == "right")
            {
                Flip(); // 중복 코드를 Flip() 함수로 대체
            }
        }
    }

    private void FixedUpdate()
    {
        // 캐시된 rbody 변수 사용
        if (direction == "right")
        {
            // Rigidbody2D의 속성은 velocity 입니다. (linearVelocity 아님)
            rbody.velocity = new Vector2(speed, rbody.velocity.y); // 오른쪽으로 이동
        }
        else
        {
            rbody.velocity = new Vector2(-speed, rbody.velocity.y); // 왼쪽으로 이동
        }
    }

    // 방향 전환 로직을 별도 함수로 분리 (코드 중복 제거)
    private void Flip()
    {
        if (direction == "right")
        {
            direction = "left";
            transform.localScale = new Vector2(1, 1); // 왼쪽을 바라보도록 설정
        }
        else
        {
            direction = "right";
            transform.localScale = new Vector2(-1, 1); // 오른쪽을 바라보도록 설정
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // "Player" 태그와 충돌했을 때만 방향을 전환 (로직 충돌 방지)
        if (collision.CompareTag("Player"))
        {
            Flip();
        }
    }
}