using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Text textHighScore;      // �ְ����� ��� �ؽ�Ʈ ������Ʈ

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true); // ȭ�� �ػ� ���� (1920x1080, ��üȭ��)
    }

    private void Start()
    {
        GameManager.totalScore = 0;     // ������ �ʱ�ȭ

        int highScore = PlayerPrefs.GetInt("HighScore", 0); // ����� �ְ����� �ҷ�����, ������ 0
        textHighScore.text = highScore.ToString(); // �ؽ�Ʈ ������Ʈ�� �ְ����� ���
    }
}
