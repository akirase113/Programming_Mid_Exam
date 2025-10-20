using UnityEngine;
using UnityEngine.UI;   // ����Ƽ���� UI�� ����ϱ� ���� ���ӽ����̽�

public class ResultManager : MonoBehaviour
{
    public GameObject scoreText;

    private void Start()
    {
        scoreText.GetComponent<Text>().text = GameManager.totalScore.ToString();

        // �ְ����� ���� (���� �������� ����� �ְ��������� ũ�� ����)
        if (PlayerPrefs.GetInt("HighScore", 0) < GameManager.totalScore)
        {
            // �ְ����� ����
            PlayerPrefs.SetInt("HighScore", GameManager.totalScore);
        }
    }
}
