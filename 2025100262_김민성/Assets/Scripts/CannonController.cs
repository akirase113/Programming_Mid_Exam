using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject objPrefab;            // 포대에서 발사되는 포탄 프리팹
    public float delayTime = 3.0f;         // 포탄 발사 간격 시간
    public float fireSpeedX = -4.0f;      // 포탄의 X축에 가해지는 힘
    public float fireSpeedY = 0.0f;       // 포탄의 Y축에 가해지는 힘
    public float length = 8.0f;         // 플레이어 감지 거리

    GameObject player;            // 플레이어 오브젝트
    GameObject gateObj;            // 대포 입구 오브젝트
    float passedTime = 0.0f;         // 경과 시간

    public AudioClip soundCannon; // 대포 발사 사운드

    private void Start()
    {
        Transform tr = transform.Find("gate");      // 자신의 자식 오브젝트 중 "gate" 이름을 가진 오브젝트 탐색
        gateObj = tr.gameObject;        // 대포 입구 오브젝트 저장
        player = GameObject.FindGameObjectWithTag("Player");   // 태그가 "Player"인 오브젝트 탐색
    }

    private void Update()
    {
        passedTime += Time.deltaTime;       // 경과 시간 갱신
        if (CheckLength(player.transform.position))     // 플레이어가 감지 거리 이내에 있으면
        {
            if (passedTime >= delayTime)    // delayTime이 경과했으면
            {
                passedTime = 0.0f;          // 경과 시간 초기화
                // 대포 입구 위치 계산
                Vector3 pos = new Vector3(gateObj.transform.position.x, gateObj.transform.position.y, transform.position.z);

                GameObject obj = Instantiate(objPrefab, pos, Quaternion.identity);  // 포탄 오브젝트 생성

                Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();        // 포탄의 Rigidbody2D 컴포넌트 얻기
                Vector2 v = new Vector2(fireSpeedX, fireSpeedY);    // 포탄에 가할 힘 벡터로 생성
                rbody.AddForce(v, ForceMode2D.Impulse);         // 포탄에 힘 주기

                AudioSource src = obj.AddComponent<AudioSource>(); // AudioSource 컴포넌트 추가
                src.clip = soundCannon;        // 오디오 클립 로드
                src.Play();
            }
        }
    }

    private bool CheckLength(Vector2 targetPos)
    {
        bool ret = false;
        float d = Vector2.Distance(transform.position, targetPos);   // 대포와 플레이어 간의 거리 계산
        if (length >= d)
        {
            ret = true;    // 플레이어가 감지 거리 이내에 있으면 true 반환
        }
        return ret;
    }
}
