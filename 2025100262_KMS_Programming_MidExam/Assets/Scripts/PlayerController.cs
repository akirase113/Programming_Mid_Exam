using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rbody = null;   // Rigidbody2D 변수
    private float axisH = 0.0f;         // 수평 입력 값

    public float speed = 3.0f;
    public float jump = 9.0f;           // 점프력
    public LayerMask groundLayer;       // 착지할수 있는 레이어
    private bool goJump = false;        // 점프 개시 플래그
    private bool onGround = false;      // 지면에 서 있는 플래그

    private Animator animator = null;       // 애니메이터
    public string stopAnime = "PlayerStop"; // 대기 애니메이션
    public string moveAnime = "PlayerMove"; // 이동 애니메이션
    public string jumpAnime = "PlayerJump"; // 점프 애니메이션
    public string goalAnime = "PlayerGoal"; // 클리어 애니메이션
    public string deadAnime = "PlayerOver"; // 게임오버 애니메이션
    private string nowAnime = ""; // 현재 재생 중인 애니메이션
    private string oldAnime = ""; // 이전에 재생 중이던 애니메이션

    public static string gameState = "playing"; // 게임 상태
    public int score = 0; // 점수

    public AudioClip soundJump; // 점프 사운드
    public AudioClip soundRanding; // 착지 사운드
    private bool isRandingSound = false;

    private void Start()
    {
        // 스크립트가 있는 게임 오브젝트의 Rigidbody2D 컴포넌트를 가져와 rbody 변수에 할당
        rbody = GetComponent<Rigidbody2D>();

        // Animator 가져오기
        animator = GetComponent<Animator>();
        nowAnime = stopAnime; // 초기 애니메이션 설정
        oldAnime = stopAnime;

        gameState = "playing"; // 게임 상태 초기화 (게임중)
    }

    private void Update()
    {
        if (gameState != "playing")
            return; // 게임 상태가 "playing"이 아니면 업데이트 중지

        axisH = Input.GetAxis("Horizontal");    // 수평 입력 값을 axisH 변수에 저장

        if (axisH > 0.0f)   // 오른쪽 이동
        {
            transform.localScale = new Vector2(1.0f, 1.0f);
        }
        else if (axisH < 0.0f)  // 왼쪽 이동
        {
            transform.localScale = new Vector2(-1.0f, 1.0f);        // 좌우 반전
        }

        // 캐릭터 점프하기
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    public void Jump()
    {
        goJump = true;  // 점프 개시 플래그 설정
        Debug.Log("점프눌림!");
    }

    private void FixedUpdate()
    {
        if (gameState != "playing")
            return; // 게임 상태가 "playing"이 아니면 업데이트 중지

        // 착지 판정
        onGround = Physics2D.Linecast(transform.position,
            transform.position - (transform.up * 0.1f),
            groundLayer);

        if (onGround && isRandingSound)     // 지면에 서 있고 착지 사운드 재생 플래그가 설정되어 있으면
        {
            GameObject obj = new GameObject("RandingSound");         // 빈 게임 오브젝트 생성
            AudioSource src = obj.AddComponent<AudioSource>();      // AudioSource 컴포넌트 추가
            src.clip = soundRanding;        // 오디오 클립 로드
            src.Play();
            Destroy(obj, src.clip.length);      // 사운드 재생이 끝나면 오브젝트 파괴
            isRandingSound = false;     // 착지 사운드 재생 플래그 해제
        }

        if (onGround || axisH != 0) // 지면 위 or 속도가 0이 아님 / 속도 갱신하기
        {
            rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);
        }

        if (onGround && goJump) // 지면 위에서 점프 키 눌림
        {
            Debug.Log("점프 개시!");
            Vector2 jumpPw = new Vector2(0.0f, jump); // 점프력 벡터
            // ForceMode2D.Impulse: 순간적인 힘 적용 , ForceMode2D.Force: 지속적인 힘 적용
            rbody.AddForce(jumpPw, ForceMode2D.Impulse); // 순간적인 힘 적용
            goJump = false; // 점프 개시 플래그 해제

            GameObject obj = new GameObject("JumpSound");         // 빈 게임 오브젝트 생성
            AudioSource src = obj.AddComponent<AudioSource>();      // AudioSource 컴포넌트 추가
            src.clip = soundJump;        // 오디오 클립 로드
            src.Play();
            Destroy(obj, src.clip.length);      // 사운드 재생이 끝나면 오브젝트 파괴

            isRandingSound = true;          // 착지 사운드 재생 플래그 설정
        }

        if (onGround)
        {
            // 지면 위
            if (axisH == 0.0f)
            {
                nowAnime = stopAnime; // 대기 애니메이션
            }
            else
            {
                nowAnime = moveAnime; // 이동 애니메이션
            }
        }
        else
        {
            // 공중
            nowAnime = jumpAnime; // 점프 애니메이션
        }

        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;        // 이전 애니메이션 갱신
            animator.Play(nowAnime);    // 애니메이션 변경            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal();
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver();
        }
        else if (collision.gameObject.tag == "ScoreItem")
        {
            // 점수 아이템
            // ItemData 가져오기
            ItemData item = collision.gameObject.GetComponent<ItemData>();
            // 점수 얻기
            score = item.value;
            // 아이템 제거
            Destroy(collision.gameObject);
            Debug.Log("점수: " + score);
        }
    }

    public void Goal()
    {
        animator.Play(goalAnime); // 클리어 애니메이션 재생

        gameState = "gameclear"; // 게임 상태 변경 (게임 클리어)
        GameStop(); // 게임 정지
    }

    public void GameOver()
    {
        animator.Play(deadAnime); // 게임오버 애니메이션 재생

        gameState = "gameover"; // 게임 상태 변경 (게임 오버)
        GameStop(); // 게임 정지

        GetComponent<CapsuleCollider2D>().enabled = false; // 충돌 비활성화
        // 위로 튕겨 오르게 하는 연출
        rbody.AddForce(new Vector2(0.0f, 5.0f), ForceMode2D.Impulse);
    }

    private void GameStop()
    {
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.linearVelocity = new Vector2(0.0f, 0.0f); // 속도 0으로 강제 정지
    }
}
