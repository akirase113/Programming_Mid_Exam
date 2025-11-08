using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections; // [선택사항] 타이핑 효과를 위해 추가

public class DialogueManager : MonoBehaviour
{
    [Header("UI 요소")]
    public TextMeshProUGUI dialogueText;

    [Header("대사 내용")]
    [TextArea(3, 10)]
    public string[] sentences;

    [Header("다음 씬 설정")]
    [Tooltip("대사가 모두 끝난 후 이동할 씬의 이름")]
    public string nextSceneName; // [수정됨] "Stage1" 하드코딩 대신 변수 사용

    [Header("타이핑 효과 (선택사항)")]
    [Tooltip("한 글자씩 타이핑되는 속도")]
    public float typingSpeed = 0.05f;

    private int index = 0;
    private bool isTyping = false; // 현재 타이핑 중인지 확인
    private Coroutine typingCoroutine; // 실행 중인 타이핑 코루틴

    void Start()
    {
        if (sentences.Length > 0)
        {
            // 씬 시작 시 첫 번째 대사 타이핑 시작
            typingCoroutine = StartCoroutine(TypeSentence(sentences[index]));
        }
    }

    void Update()
    {
        // 마우스 왼쪽 버튼을 클릭했을 때
        if (Input.GetMouseButtonDown(0))
        {
            // 1. 만약 현재 대사가 타이핑 중이라면 -> 즉시 완료 처리
            if (isTyping)
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine); // 타이핑 중단
                }
                dialogueText.text = sentences[index]; // 전체 문장 표시
                isTyping = false;
            }
            // 2. 타이핑이 끝난 상태라면 -> 다음 대사로
            else
            {
                NextSentence();
            }
        }
    }

    // 다음 대사로 넘어가는 함수
    void NextSentence()
    {
        index++; // 다음 대사 인덱스로

        // 아직 대사가 남아있다면
        if (index < sentences.Length)
        {
            // 다음 대사 타이핑 시작
            typingCoroutine = StartCoroutine(TypeSentence(sentences[index]));
        }
        else // 모든 대사가 끝났다면
        {
            // [수정됨] 설정해둔 nextSceneName으로 씬 이동
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("다음 씬이 지정되지 않았습니다!");
            }
        }
    }

    // 한 글자씩 타이핑하는 코루틴
    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = ""; // 텍스트 초기화
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
}