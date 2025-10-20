using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rbody = null;   // Rigidbody2D 변수
    private float axisH = 0.0f;         // 수평 입력 값

    private void Start()
    {
        // 스크립트가 있는 게임 오브젝트의 Rigidbody2D 컴포넌트를 가져와 rbody 변수에 할당
        rbody = GetComponent<Rigidbody2D>();
    }

    private float speed = 3.0f;          // 이동 속도

    private void Update()
    {
        axisH = Input.GetAxis("Horizontal");    // 수평 입력 값을 axisH 변수에 저장

        Debug.Log("axisH: " + axisH);  // 수평 입력 값 출력
        // 방향 조절
        if (axisH > 0.0f)   // 오른쪽 이동
        {
            Debug.Log("오른쪽 이동");
            transform.localScale = new Vector2(1.0f, 1.0f);
            //GetComponent<SpriteRenderer>().flipX = false; // 좌우 반전 해제
        }
        else if (axisH < 0.0f)  // 왼쪽 이동
        {
            Debug.Log("왼쪽 이동");
            transform.localScale = new Vector2(-1.0f, 1.0f);        // 좌우 반전
            //GetComponent<SpriteRenderer>().flipX = true;  // 좌우 반전 적용
        }
    }

    private void FixedUpdate()
    {
        // Rigidbody2D의 속도를 설정하여 플레이어 이동
        //rbody.velocity = new Vector2(axisH * 3.0f, rbody.velocity.y);
        rbody.linearVelocity = new Vector2(axisH * 3.0f, rbody.linearVelocity.y);
    }

}