using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;          // 적의 이동 속도
    public string direction = "left";   // 적의 이동 방향 (left, right)
    public float range = 2.0f;          // 움직이는 범위
    private Vector3 defPos;             // 시작 위치

    private void Start()
    {
        if (direction == "right")
        {
            transform.localScale = new Vector2(-1, 1);      // 오른쪽을 바라보도록 설정
        }
        else
        {
            transform.localScale = new Vector2(1, 1);       // 왼쪽을 바라보도록 설정
        }
        defPos = transform.position;    // 시작 위치 저장
    }

    private void Update()
    {
        if (range > 0.0f)
        {
            // range / 2 를 해서 시작위치에서 좌우로 절반씩 범위를 가짐
            if (transform.position.x < defPos.x - (range / 2))      // 왼쪽 끝에 도달했는지 확인
            {
                direction = "right";
                transform.localScale = new Vector2(-1, 1);      // 오른쪽을 바라보도록 설정
            }
            else if (transform.position.x > defPos.x + (range / 2)) // 오른쪽 끝에 도달했는지 확인
            {
                direction = "left";
                transform.localScale = new Vector2(1, 1);       // 왼쪽을 바라보도록 설정
            }
        }
    }

    private void FixedUpdate()
    {
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();        // Rigidbody2D 컴포넌트 가져오기

        if (direction == "right")
        {
            rbody.linearVelocity = new Vector2(speed, rbody.linearVelocity.y);  // 오른쪽으로 이동
        }
        else
        {
            rbody.linearVelocity = new Vector2(-speed, rbody.linearVelocity.y); // 왼쪽으로 이동
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌 시 이동 방향 전환
        if (direction == "right")
        {
            direction = "left";
            transform.localScale = new Vector2(1, 1);       // 왼쪽을 바라보도록 설정
        }
        else
        {
            direction = "right";
            transform.localScale = new Vector2(-1, 1);      // 오른쪽을 바라보도록 설정
        }
    }
}
