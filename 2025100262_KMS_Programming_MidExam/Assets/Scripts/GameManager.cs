using UnityEngine;
using UnityEngine.UI;   // ����Ƽ���� UI�� ����ϱ� ���� ���ӽ����̽�

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;        // �̹����� ��Ƶδ� GameObject ����
    public Sprite gameOverSpr;          // Game Over �̹���
    public Sprite gameClearSpr;         // Game Clear �̹���
    public GameObject panel;            // �г�
    public GameObject restartButton;    // Restart ��ư
    public GameObject nextButton;       // Next ��ư

    Image titleImage;                   // �̹����� ǥ���ϴ� Image ������Ʈ

    public GameObject timeBar;          // �ð� ǥ�� �̹���
    public GameObject timeText;         // �ð� ǥ�� �ؽ�Ʈ
    private TimeController timeCnt;     // TimeController

    public GameObject scoreText;        // ���� ǥ�� �ؽ�Ʈ
    public static int totalScore = 0;    // �� ����
    public int stageScore = 0;          // �������� ����

    public AudioClip meGameOver;     // ���� ���� ����
    public AudioClip meGameClear;    // ���� Ŭ���� ����

    private void Start()
    {
        // �̹��� �����
        Invoke("InactiveImage", 1.0f); // 1�� �� InactiveImage �Լ� ȣ��
        // ��ư(�г�)�� �����
        panel.SetActive(false);

        // TimeContoroller ��������
        timeCnt = GetComponent<TimeController>();
        if (timeCnt != null)
        {
            if (timeCnt.gameTime == 0.0f)
            {
                // �ð� ������ ������ �ð� ǥ�� �����
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
            // ���� Ŭ����
            mainImage.SetActive(true); // �̹��� ǥ��
            panel.SetActive(true); // ��ư(�г�) ǥ��
            // Restart ��ư�� ��Ȱ��ȭ
            Button bt = restartButton.GetComponent<Button>();   // Button ������Ʈ ��������
            bt.interactable = false;    // ��ư ��Ȱ��ȭ
            mainImage.GetComponent<Image>().sprite = gameClearSpr; // ���� Ŭ���� �̹��� ����
            PlayerController.gameState = "gameend"; // ���� ����

            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;  // �ð� ī��Ʈ ����

                // ������ �Ҵ��Ͽ� �Ҽ����� ������
                int time = (int)timeCnt.displayTime;
                totalScore += (time * 10); // ���� �ð� x 10 ���� �߰�
            }

            totalScore += stageScore;   // �������� ������ �� ������ �߰�
            stageScore = 0;             // �������� ���� �ʱ�ȭ
            UpdateScore(); // ���� ����

            AudioSource soundPlayer = GetComponent<AudioSource>();  // AudioSource ������Ʈ ��������
            if(soundPlayer != null)
            {
                soundPlayer.Stop();               // ������� ����
                soundPlayer.PlayOneShot(meGameClear); // ���� Ŭ���� ���� ���
            }
        }
        else if (PlayerController.gameState == "gameover")
        {
            // ���� ����
            mainImage.SetActive(true);
            panel.SetActive(true);
            // Next��ư�� ��Ȱ��ȭ
            Button bt = nextButton.GetComponent<Button>();   // Button ������Ʈ ��������
            bt.interactable = false;    // ��ư ��Ȱ��ȭ
            mainImage.GetComponent<Image>().sprite = gameOverSpr; // ���� ���� �̹��� ����
            PlayerController.gameState = "gameend"; // ���� ����

            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;  // �ð� ī��Ʈ ����
            }

            AudioSource soundPlayer = GetComponent<AudioSource>();  // AudioSource ������Ʈ ��������
            if (soundPlayer != null)
            {
                soundPlayer.Stop();               // ������� ����
                soundPlayer.PlayOneShot(meGameOver); // ���� ���� ���� ���
            }
        }
        else if (PlayerController.gameState == "playing")
        {
            // ���� ��
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            // PlayerController ��������
            PlayerController playerCnt = player.GetComponent<PlayerController>();
            // �ð� ����
            if (timeCnt != null)
            {
                if (timeCnt.gameTime > 0.0f)
                {
                    // ������ �Ҵ��Ͽ� �Ҽ��� ���ϸ� ����
                    int time = (int)timeCnt.displayTime;
                    // �ð� ����
                    timeText.GetComponent<Text>().text = time.ToString();
                    // Ÿ�ӿ���
                    if (time == 0)
                    {
                        playerCnt.GameOver(); // ���� ����
                    }
                }
            }

            if (playerCnt.score != 0)
            {
                stageScore += playerCnt.score;  // ������ ȹ���� �������� �������� ������ �߰�
                playerCnt.score = 0;        // ������ ȹ���� ������ ���� �ʱ�ȭ
                UpdateScore(); // ���� ����
            }
        }
    }

    private void InactiveImage()
    {
        mainImage.SetActive(false);
    }
}
