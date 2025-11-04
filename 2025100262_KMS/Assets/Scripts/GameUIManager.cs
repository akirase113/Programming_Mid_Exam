using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필수!
using System.Collections; // 코루틴(시간 지연)을 위해 필수!

public class GameUIManager : MonoBehaviour
{
    [Header("UI 오브젝트 연결")]
    public GameObject gameStartImage1;   // 1초간 보일 'Game Start' 이미지
    public GameObject gameStartImage2;   // 1초간 보일 'Game Start' 이미지
    public GameObject pauseMenuPanel;   // 'ESC' 누르면 보일 일시정지 패널


    private bool isPaused = false; // 현재 일시정지 상태인지 확인

    // 씬이 시작될 때 1회 호출
    void Start()
    {
        // 1. 필요한 UI들을 초기 상태로 설정
        gameStartImage1.SetActive(true);
        gameStartImage2.SetActive(true);
        pauseMenuPanel.SetActive(false);
        
        // 2. 게임 시간 흐르게 (필수)
        Time.timeScale = 1f;
        isPaused = false;

        // 3. "1초 뒤에 GameStartImage를 숨겨라"라고 명령
        StartCoroutine(HideStartImageAfterDelay(1.0f));
    }

    // 1초간 기다렸다가 이미지를 숨기는 함수 (코루틴)
    private IEnumerator HideStartImageAfterDelay(float delay)
    {
        // 'delay' 시간만큼 기다림
        yield return new WaitForSeconds(delay);

        // 이미지를 비활성화
        gameStartImage1.SetActive(false);
        gameStartImage2.SetActive(false);
    }

    // 매 프레임마다 키 입력을 감지
    void Update()
    {
        // 'ESC' 키가 눌렸는지 확인
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                // 이미 일시정지 상태면, 게임 재개
                ResumeGame();
            }
            else
            {
                // 일시정지 상태가 아니면, 게임 일시정지
                PauseGame();
            }
        }
    }

    // --- (아래 함수들은 버튼에서 호출할 예정) ---

    // 게임을 일시정지하는 함수
    public void PauseGame()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true); // 일시정지 메뉴를 켬
        Time.timeScale = 0f; // 게임 내의 시간을 0배속으로 멈춤 (물리, 애니메이션 등)
    }

    // [계속하기] 버튼이 호출할 함수
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false); // 일시정지 메뉴를 끔
        Time.timeScale = 1f; // 게임 시간을 다시 1배속으로 되돌림
    }

    // [타이틀 화면으로] 버튼이 호출할 함수
    public void GoToTitle()
    {
        // 중요: 씬을 넘어가기 전, 멈췄던 시간을 반드시 1로 되돌려야 합니다!
        Time.timeScale = 1f;
        
        // StartScene (빌드 설정 0번 씬)으로 돌아감
        SceneManager.LoadScene("StartScene"); 
    }
}