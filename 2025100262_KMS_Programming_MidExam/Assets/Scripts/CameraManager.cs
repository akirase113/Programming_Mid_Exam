using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float leftLimit = 0.0f;      // 왼쪽 스크롤 제한
    public float rightLimit = 0.0f;     // 오른쪽 스크롤 제한
    public float topLimit = 0.0f;       // 위쪽 스크롤 제한
    public float bottomLimit = 0.0f;    // 아래쪽 스크롤 제한

    public GameObject subScreen;     // 서브 스크린 오브젝트

    public bool isForceScrollX = false;     // X축 강제 스크롤 플래그
    public float forceScrollSpeedX = 0.5f;  // 1초간 움직일 X축 거리
    public bool isForceScrollY = false;     // Y축 강제 스크롤 플래그
    public float forceScrollSpeedY = 0.5f;  // 1초간 움직일 Y축 거리

    private void Update()
    {
        // 플레이어 찾기 (태그가 "Player"인 게임 오브젝트)
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 카메라 좌표 갱신
            float x = player.transform.position.x;  // 플레이어의 x 좌표를 카메라의 x 좌표로 설정
            float y = player.transform.position.y;  // 플레이어의 y 좌표를 카메라의 y 좌표로 설정
            float z = transform.position.z; // 카메라의 z 좌표는 그대로 유지

            // 가로방향 동기화
            if (isForceScrollX)
            {
                // 강제 스크롤 적용
                x = transform.position.x + (forceScrollSpeedX * Time.deltaTime);
            }

            // 좌우 끝에 이동 제한 적용        
            if (x < leftLimit)
            {
                x = leftLimit;
            }
            else if (x > rightLimit)
            {
                x = rightLimit;
            }

            // 세로방향 동기화
            if (isForceScrollY)
            {
                // 강제 스크롤 적용
                y = transform.position.y + (forceScrollSpeedY * Time.deltaTime);
            }

            // 위아래 끝에 이동 제한 적용
            if (y < bottomLimit)
            {
                y = bottomLimit;
            }
            else if (y > topLimit)
            {
                y = topLimit;
            }

            // 카메라 위치의 Vector3 생성
            Vector3 v3 = new Vector3(x, y, z);
            transform.position = v3; // 카메라 위치 갱신

            // 서브 스크린 스크롤
            if (subScreen != null)
            {
                // 서브 스크린 위치 갱신
                y = subScreen.transform.position.y;
                z = subScreen.transform.position.z;
                Vector3 v = new Vector3(x / 2.0f, y, z);    // 서브 스크린은 카메라의 절반 속도로 이동
                subScreen.transform.position = v; // 서브 스크린 위치 갱신
            }
        }
    }
}
