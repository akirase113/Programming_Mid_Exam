using UnityEngine;

public class GimmickBlock : MonoBehaviour
{
    public float length = 0.0f;         // 자동 낙하 탐지 거리
    public bool isDelete = false;       // 낙하 후 제거할지 여부

    bool isFell = false;                // 낙하 플래그
    float fadeTime = 0.5f;               // 페이드 아웃 시간

    private void Start()
    {
        // Rigidbody2D 물리 시뮬레이션 정지
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;
    }

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     // 플레이어 찾기

        if (player != null)
        {
            // 플레이어와의 거리 계산 (오브젝트와 플레이어의 위치 차이의 제곱의 합의 제곱근)
            float d = Vector2.Distance(transform.position, player.transform.position);

            if (length >= d)
            {
                Rigidbody2D rbody = GetComponent<Rigidbody2D>();
                if (rbody.bodyType == RigidbodyType2D.Static)   // 현재 물리 시뮬레이션이 정지 상태인 경우                
                {
                    rbody.bodyType = RigidbodyType2D.Dynamic;   // Rigidbody2D 물리 시뮬레이션 시작
                }
            }
        }

        if (isFell) // 낙하 후 제거 플래그가 설정된 경우
        {
            fadeTime -= Time.deltaTime;     // 시간이 지남에 따라
            Color col = GetComponent<SpriteRenderer>().color;   // 스프라이트 렌더러의 색상 가져오기
            col.a = fadeTime;               // 알파 값을 페이드 타임으로 설정
            GetComponent<SpriteRenderer>().color = col; // 색상 업데이트
            if (fadeTime <= 0.0f)   // 페이드 타임이 0 이하가 되면
            {
                Destroy(gameObject);    // 오브젝트 제거
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDelete)
        {
            isFell = true;  // 낙하 플래그 설정            
        }
    }
}
