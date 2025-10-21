using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환에 필요

public class Goal : MonoBehaviour
{
    // 이 Goal 오브젝트의 Trigger Collider 안에 다른 Collider가 들어오는 순간 1회 호출됨
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // 들어온 오브젝트의 Tag가 "Player"인지 확인
        if (other.CompareTag("Player"))
        {
            Debug.Log("STAGE CLEAR!"); // 콘솔에 클리어 메시지 출력

            // 2단계에서 만든 PlayerController 스크립트를 가져옴
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // 플레이어의 클리어 함수 호출 (애니메이션 재생, 조작 비활성화 등)
                player.Clear(); 
            }

            // (선택) 2초 후에 다음 씬으로 넘어가기
            // Invoke("LoadNextStage", 2f); 
            // (5단계에서 씬 관리를 배울 것이므로 지금은 Debug.Log만으로도 충분합니다)
        }
    }

    /*
    // 다음 스테이지 로드 함수
    void LoadNextStage()
    {
        // TODO: 현재 씬 이름을 기반으로 다음 씬 이름 알아내기
        // 예: SceneManager.LoadScene("Stage2");
        // 중요: 다음 씬(Stage2)이 File > Build Settings에 등록되어 있어야 함
    }
    */
}