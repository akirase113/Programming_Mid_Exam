using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Text textHighScore;      // 최고점수 출력 텍스트 컴포넌트

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true); // 화면 해상도 설정 (1920x1080, 전체화면)
    }

    private void Start()
    {
        GameManager.totalScore = 0;     // 총점수 초기화

        int highScore = PlayerPrefs.GetInt("HighScore", 0); // 저장된 최고점수 불러오기, 없으면 0
        textHighScore.text = highScore.ToString(); // 텍스트 컴포넌트에 최고점수 출력
    }
}
