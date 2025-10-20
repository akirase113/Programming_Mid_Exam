using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float moveX = 0.0f;       // X축 이동 거리
    public float moveY = 0.0f;       // Y축 이동 거리
    public float times = 0.0f;      // 움직이는 시간
    public float weight = 0.0f;     // 대기 시간
    public bool isMoveWhenOn = false; // 플레이어가 올라갔을 때 이동 여부

    public bool isCanMove = true;
    private float perDX;        // 1프레임당 X이동 값
    private float perDY;        // 1프레임당 Y이동 값
    Vector3 defPos;      // 초기 위치
    bool isReverse = false;  // 이동 방향 플래그

    private void Start()
    {
        defPos = transform.position;            // 초기위치 저장
        float timestep = Time.fixedDeltaTime;   // 1프레임 시간 간격 (기본값 0.02초)
        perDX = moveX / (times / timestep * times); // 1프레임당 X이동 값 계산
        perDY = moveY / (times / timestep * times); // 1프레임당 Y이동 값 계산

        if (isMoveWhenOn)
        {
            isCanMove = false; // 플레이어가 올라갔을 때 이동하는 경우 초기에는 이동하지 않도록 설정
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(transform); // 플레이어를 이동 블록의 자식으로 설정하여 함께 이동하도록 함
        }
        if (isMoveWhenOn)
        {
            isCanMove = true; // 플레이어가 올라갔을 때 이동 시작
        }
        Debug.Log("이동블록에 캐릭터가 올라감");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // 플레이어를 이동 블록의 자식이 아니게 설정하여 블록이동에 영향받지 않도록 함
            collision.transform.SetParent(null);
        }
        Debug.Log("이동블록에서 캐릭터가 내려감");
    }

    private void FixedUpdate()
    {
        if (isCanMove)
        {
            float x = transform.position.x;     // 현재 위치의 X좌표
            float y = transform.position.y;     // 현재 위치의 Y좌표
            bool endX = false;        // X축 이동 종료 플래그
            bool endY = false;        // Y축 이동 종료 플래그

            if (isReverse)
            {// 역방향 이동
                // 이동량이 양수고 이동 위치가 초기 위치보다 작거나
                // 이동량이 음수고 이동 위치가 초기 + 이동거리 보다 큰 경우
                if ((perDX >= 0.0f && x <= defPos.x) || (perDX < 0.0f && x >= defPos.x))
                {
                    endX = true;    // X축 이동 종료
                }
                if ((perDY >= 0.0f && y <= defPos.y) || (perDY < 0.0f && y >= defPos.y))
                {
                    endY = true;    // Y축 이동 종료
                }

                // 블록 이동
                Vector3 v = new Vector3(-perDX, -perDY, defPos.z);
                transform.Translate(v);
            }
            else
            {// 정방향 이동
                // 이동량이 양수고 이동 위치가 초기 위치보다 크거나
                // 이동량이 음수고 이동 위치가 초기 + 이동거리 보다 작은 경우
                if ((perDX >= 0.0f && x >= defPos.x + moveX) || (perDX < 0.0f && x <= defPos.x + moveX))
                {
                    endX = true;    // X축 이동 종료
                }
                if ((perDY >= 0.0f && y >= defPos.y + moveY) || (perDY < 0.0f && y <= defPos.y + moveY))
                {
                    endY = true;    // Y축 이동 종료
                }

                // 블록 이동
                Vector3 v = new Vector3(perDX, perDY, defPos.z);
                transform.Translate(v);
            }

            if (endX && endY)
            {
                if (isReverse)  // 역방향으로 이동중이였다면
                {
                    transform.position = defPos; // 초기 위치로 이동
                }

                isReverse = !isReverse; // 이동 방향 전환
                isCanMove = false;      // 이동 정지
                if (isMoveWhenOn == false)      // 플레이어가 올라갔을 때 이동하는 경우가 아니라면
                {
                    Invoke("Move", weight);         // weight 만큼 대기 후 이동 재개
                }
            }
        }
    }

    public void Move()
    {
        isCanMove = true;
    }

    public void Stop()
    {
        isCanMove = false;
    }
}
