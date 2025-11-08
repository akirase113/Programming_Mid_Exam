using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; // TextMeshPro를 사용하는 경우

public class Goal : MonoBehaviour
{
    [Header("클리어 설정")]
    [Tooltip("클리어 애니메이션 후 다음 씬으로 넘어갈 딜레이")]
    public float clearDelay = 2.0f;
    [Tooltip("전체 스테이지 수 (예: 4)")]
    public int totalStages = 4;
    [Tooltip("마지막 스테이지 클리어 시 이동할 씬 이름")]
    public string epilogueSceneName = "EpilogueScene";

    [Header("UI 연결 (선택 사항)")]
    [Tooltip("적이 남았을 때 표시할 경고 메시지 텍스트")]
    public TextMeshProUGUI warningText;
    [Tooltip("경고 메시지 표시 시간")]
    public float warningDuration = 1.5f;

    // 중복 클리어 방지용 플래그
    private bool isCleared = false;

    void Start()
    {
        // 경고 메시지가 있다면 시작 시 숨김
        if (warningText != null)
        {
            warningText.gameObject.SetActive(false);
        }
    }

    // 트리거 감지
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 이미 클리어했거나 플레이어가 아니면 무시
        if (isCleared || !other.CompareTag("Player"))
        {
            return;
        }

        // 1. [수정됨] GameUIManager에게 적이 다 죽었는지 물어봄
        if (GameUIManager.instance != null && GameUIManager.instance.AreAllEnemiesDead())
        {
            // [성공] 적이 다 죽었음 -> 클리어 진행
            isCleared = true; // 중복 실행 방지
            Debug.Log("모든 적 처치! STAGE CLEAR!");

            // 2. 플레이어 클리어 애니메이션 호출
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Clear();
            }

            // 3. [수정됨] GameUIManager에게 'Stage Clear' 이미지를 켜달라고 요청
            if (GameUIManager.instance != null)
            {
                GameUIManager.instance.ShowStageClear();
            }

            // 4. 다음 스테이지 로드 코루틴 시작
            StartCoroutine(LoadNextStageAfterDelay(clearDelay));
        }
        else
        {
            // [실패] 아직 적이 남아있음
            Debug.Log("아직 적이 남아있습니다! 클리어할 수 없습니다.");
            if (warningText != null)
            {
                StartCoroutine(ShowWarningMessage());
            }
        }
    }

    // 경고 메시지를 잠깐 보여주고 숨기는 코루틴
    private IEnumerator ShowWarningMessage()
    {
        warningText.text = "남아있는 적을 모두 처치해야 합니다!";
        warningText.gameObject.SetActive(true);

        yield return new WaitForSeconds(warningDuration);

        warningText.gameObject.SetActive(false);
    }

    // 딜레이 후 다음 스테이지로 이동하는 코루틴
    private IEnumerator LoadNextStageAfterDelay(float delay)
    {
        // 설정한 딜레이(clearDelay)만큼 기다림
        yield return new WaitForSeconds(delay);

        // 현재 씬 이름에서 스테이지 번호 추출 (예: "Stage1" -> 1)
        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentStageNum = 0;

        // 씬 이름이 "Stage<번호>" 형식이 아니면 오류 발생 가능
        try
        {
            currentStageNum = int.Parse(currentSceneName.Replace("Stage", ""));
        }
        catch
        {
            Debug.LogError("씬 이름 형식이 잘못되었습니다! 'Stage<번호>' 형식이어야 합니다.");
            yield break; // 코루틴 중단
        }

        // 마지막 스테이지인지 확인
        if (currentStageNum == totalStages)
        {
            // [4스테이지 클리어] -> 에필로그 씬으로
            Debug.Log("모든 스테이지 클리어! 에필로그로 이동합니다.");
            PlayerPrefs.SetInt("CurrentStage", totalStages); // 이어하기 정보 저장
            PlayerPrefs.Save();
            SceneManager.LoadScene(epilogueSceneName);
        }
        else
        {
            // [중간 스테이지 클리어] -> 다음 스테이지로
            int nextStageNum = currentStageNum + 1;
            string nextSceneName = "Stage" + nextStageNum;

            PlayerPrefs.SetInt("CurrentStage", nextStageNum); // 이어하기 정보 저장
            PlayerPrefs.Save();

            SceneManager.LoadScene(nextSceneName);
        }
    }
}