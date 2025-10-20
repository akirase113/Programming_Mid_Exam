using UnityEngine;
using UnityEngine.UI;   // 유니티에서 UI를 사용하기 위한 네임스페이스

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;        // 이미지를 담아두는 GameObject 변수
    public Sprite gameOverSpr;          // Game Over 이미지
    public Sprite gameClearSpr;         // Game Clear 이미지
    public GameObject panel;            // 패널
    public GameObject restartButton;    // Restart 버튼
    public GameObject nextButton;       // Next 버튼

    Image titleImage;                   // 이미지를 표시하는 Image 컴포넌트

    public GameObject timeBar;          // 시간 표시 이미지
    public GameObject timeText;         // 시간 표시 텍스트
    private TimeController timeCnt;     // TimeController

    public GameObject scoreText;        // 점수 표시 텍스트
    public static int totalScore = 0;    // 총 점수
    public int stageScore = 0;          // 스테이지 점수

    public AudioClip meGameOver;     // 게임 오버 사운드
    public AudioClip meGameClear;    // 게임 클리어 사운드

    private void Start()
    {
        // 이미지 숨기기
        Invoke("InactiveImage", 1.0f); // 1초 후 InactiveImage 함수 호출
        // 버튼(패널)을 숨기기
        panel.SetActive(false);

        // TimeContoroller 가져오기
        timeCnt = GetComponent<TimeController>();
        if (timeCnt != null)
        {
            if (timeCnt.gameTime == 0.0f)
            {
                // 시간 제한이 없으면 시간 표시 숨기기
                timeBar.SetActive(false);
            }
        }

        UpdateScore();
    }

    private void UpdateScore()
    {
        int score = stageScore + totalScore;
        scoreText.GetComponent<Text>().text = score.ToString();
    }

    private void Update()
    {
        if (PlayerController.gameState == "gameclear")
        {
            // 게임 클리어
            mainImage.SetActive(true); // 이미지 표시
            panel.SetActive(true); // 버튼(패널) 표시
            // Restart 버튼을 비활성화
            Button bt = restartButton.GetComponent<Button>();   // Button 컴포넌트 가져오기
            bt.interactable = false;    // 버튼 비활성화
            mainImage.GetComponent<Image>().sprite = gameClearSpr; // 게임 클리어 이미지 설정
            PlayerController.gameState = "gameend"; // 게임 종료

            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;  // 시간 카운트 중지

                // 정수에 할당하여 소수점을 버린다
                int time = (int)timeCnt.displayTime;
                totalScore += (time * 10); // 남은 시간 x 10 점수 추가
            }

            totalScore += stageScore;   // 스테이지 점수를 총 점수에 추가
            stageScore = 0;             // 스테이지 점수 초기화
            UpdateScore(); // 점수 갱신

            AudioSource soundPlayer = GetComponent<AudioSource>();  // AudioSource 컴포넌트 가져오기
            if(soundPlayer != null)
            {
                soundPlayer.Stop();               // 배경음악 정지
                soundPlayer.PlayOneShot(meGameClear); // 게임 클리어 사운드 재생
            }
        }
        else if (PlayerController.gameState == "gameover")
        {
            // 게임 오버
            mainImage.SetActive(true);
            panel.SetActive(true);
            // Next버튼을 비활성화
            Button bt = nextButton.GetComponent<Button>();   // Button 컴포넌트 가져오기
            bt.interactable = false;    // 버튼 비활성화
            mainImage.GetComponent<Image>().sprite = gameOverSpr; // 게임 오버 이미지 설정
            PlayerController.gameState = "gameend"; // 게임 종료

            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;  // 시간 카운트 중지
            }

            AudioSource soundPlayer = GetComponent<AudioSource>();  // AudioSource 컴포넌트 가져오기
            if (soundPlayer != null)
            {
                soundPlayer.Stop();               // 배경음악 정지
                soundPlayer.PlayOneShot(meGameOver); // 게임 오버 사운드 재생
            }
        }
        else if (PlayerController.gameState == "playing")
        {
            // 게임 중
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            // PlayerController 가져오기
            PlayerController playerCnt = player.GetComponent<PlayerController>();
            // 시간 갱신
            if (timeCnt != null)
            {
                if (timeCnt.gameTime > 0.0f)
                {
                    // 정수에 할당하여 소수점 이하를 버림
                    int time = (int)timeCnt.displayTime;
                    // 시간 갱신
                    timeText.GetComponent<Text>().text = time.ToString();
                    // 타임오버
                    if (time == 0)
                    {
                        playerCnt.GameOver(); // 게임 오버
                    }
                }
            }

            if (playerCnt.score != 0)
            {
                stageScore += playerCnt.score;  // 유저가 획득한 아이템을 스테이지 점수에 추가
                playerCnt.score = 0;        // 유저가 획득한 아이템 점수 초기화
                UpdateScore(); // 점수 갱신
            }
        }
    }

    private void InactiveImage()
    {
        mainImage.SetActive(false);
    }
}
