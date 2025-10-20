using UnityEngine;
using UnityEngine.UI;   // 유니티에서 UI를 사용하기 위한 네임스페이스

public class ResultManager : MonoBehaviour
{
    public GameObject scoreText;

    private void Start()
    {
        scoreText.GetComponent<Text>().text = GameManager.totalScore.ToString();

        // 최고점수 갱신 (현재 총점수가 저장된 최고점수보다 크면 갱신)
        if (PlayerPrefs.GetInt("HighScore", 0) < GameManager.totalScore)
        {
            // 최고점수 저장
            PlayerPrefs.SetInt("HighScore", GameManager.totalScore);
        }
    }
}
