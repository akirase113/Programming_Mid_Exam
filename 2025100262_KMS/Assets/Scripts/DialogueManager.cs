using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리
using TMPro; // TextMeshPro 사용

public class DialogueManager : MonoBehaviour
{
    [Header("UI 요소")]
    // Inspector 창에서 대화 텍스트 UI를 연결
    public TextMeshProUGUI dialogueText; 

    [Header("대사 내용")]
    // Inspector 창에서 출력할 대사들을 미리 입력
    [TextArea(3, 10)]
    public string[] sentences;

    private int index = 0; // 현재 몇 번째 대사인지 추적

    // 씬이 시작되면 첫 번째 대사를 표시
    void Start()
    {
        if (sentences.Length > 0)
        {
            dialogueText.text = sentences[index];
        }
    }

    // 매 프레임마다 마우스 클릭을 감지
    void Update()
    {
        // 마우스 왼쪽 버튼을 클릭했을 때
        if (Input.GetMouseButtonDown(0)) 
        {
            NextSentence();
        }
    }

    void NextSentence()
    {
        index++; // 다음 대사 인덱스로

        // 아직 대사가 남아있다면
        if (index < sentences.Length)
        {
            dialogueText.text = sentences[index];
        }
        else // 모든 대사가 끝났다면
        {
            // Stage1로 씬 이동
            SceneManager.LoadScene("Stage1");
        }
    }
}