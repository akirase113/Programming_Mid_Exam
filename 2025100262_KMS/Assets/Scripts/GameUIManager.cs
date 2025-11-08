using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; // TextMeshPro를 사용하는 경우

public class GameUIManager : MonoBehaviour
{
    [Header("UI 오브젝트 연결")]
    public GameObject gameStartImage1;
    public GameObject gameStartImage2;
    public GameObject pauseMenuPanel;
    public GameObject gameOverImage;
    public GameObject stageClearImage; // [추가] 클리어 시 보일 이미지

    [Header("게임 관리")]
    // 싱글톤 인스턴스: 다른 스크립트가 쉽게 접근할 수 있게 함
    public static GameUIManager instance;

    // 현재 씬에 남은 적의 수
    private int enemiesRemaining = 0;

    void Awake()
    {
        // 싱글톤 패턴 설정
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // 이미 인스턴스가 존재하면 이 오브젝트는 파괴
            Destroy(gameObject);
        }
    }

    // 씬이 시작될 때 1회 호출
    void Start()
    {
        // 1. UI 초기화
        gameStartImage1.SetActive(true);
        gameStartImage2.SetActive(true);
        pauseMenuPanel.SetActive(false);
        gameOverImage.SetActive(false);
        stageClearImage.SetActive(false); // 클리어 이미지 숨김

        // 2. 게임 시간 흐르게
        Time.timeScale = 1f;

        // 3. "Game Start" 이미지 숨기기 코루틴 실행
        StartCoroutine(HideStartImageAfterDelay(1.0f));

        // 4. 현재 씬의 적 수 카운트
        // "Enemy" 태그를 가진 모든 오브젝트를 찾아 숫자를 세기
        enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("현재 스테이지의 적 수: " + enemiesRemaining);
    }

    // "Game Start" 이미지를 1초 뒤에 숨김
    private IEnumerator HideStartImageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (gameStartImage1!= null)
        {
            gameStartImage1.SetActive(false);
        }
        if (gameStartImage2 != null)
        {
            gameStartImage2.SetActive(false);
        }
    }

    // 매 프레임마다 키 입력을 감지
    void Update()
    {
        // 'ESC' 키가 눌렸는지 확인
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Time.timeScale이 0f이면 (멈춘 상태면)
            if (Time.timeScale == 0f)
            {
                ResumeGame(); // 게임 재개
            }
            else
            {
                PauseGame(); // 게임 일시정지
            }
        }
    }

    // --- 적 관리 함수 ---

    // Enemy.cs가 호출할 함수
    public void EnemyDefeated()
    {
        enemiesRemaining--; // 남은 적 수 1 감소
        Debug.Log("적 처치! 남은 적: " + enemiesRemaining);
    }

    // Goal.cs가 호출할 함수
    public bool AreAllEnemiesDead()
    {
        // 남은 적이 0 이하인지 확인
        return enemiesRemaining <= 0;
    }

    // --- UI 제어 함수 ---

    // 게임을 일시정지하는 함수
    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true); // 일시정지 메뉴를 켬
        Time.timeScale = 0f; // 게임 내의 시간을 0배속으로 멈춤
    }

    // [계속하기] 버튼이 호출할 함수
    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false); // 일시정지 메뉴를 끔
        Time.timeScale = 1f; // 게임 시간을 다시 1배속으로 되돌림
    }

    // [타이틀 화면으로] 버튼이 호출할 함수
    public void GoToTitle()
    {
        Time.timeScale = 1f; // 멈췄던 시간을 반드시 1로 되돌림
        SceneManager.LoadScene("StartScene"); // StartScene으로 돌아감
    }

    // PlayerController가 호출할 함수
    public void ShowGameOver()
    {
        pauseMenuPanel.SetActive(false);
        gameOverImage.SetActive(true); // 'Game Over' 이미지를 활성화
    }

    // Goal.cs가 호출할 함수
    public void ShowStageClear()
    {
        pauseMenuPanel.SetActive(false);
        stageClearImage.SetActive(true); // 'Stage Clear' 이미지를 활성화
    }
}