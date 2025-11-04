using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필수
using System.Collections; // 시간 지연(Coroutine)을 위해 필수

public class Goal : MonoBehaviour
{
    [Header("클리어 설정")]
    [Tooltip("클리어 후 다음 씬으로 넘어가기까지 대기 시간(초)")]
    public float clearDelay = 2.0f; // 플레이어 클리어 애니메이션 시간

    [Tooltip("총 스테이지 개수 (이 개수에 도달하면 에필로그 씬으로)")]
    public int totalStages = 4;

    [Tooltip("마지막 스테이지 클리어 후 이동할 씬 이름")]
    public string epilogueSceneName = "EpilogueScene";

    // 중복 클리어 방지 플래그
    private bool isCleared = false;

    /// <summary>
    /// 이 Goal 오브젝트의 Trigger Collider 안에 다른 Collider가 들어오는 순간 1회 호출됨
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 이미 클리어했거나, 들어온 것이 "Player" 태그가 아니면 무시
        if (isCleared || !other.CompareTag("Player"))
        {
            return;
        }

        // 2. 클리어 상태로 변경 (중복 실행 방지)
        isCleared = true;
        Debug.Log("STAGE CLEAR!");

        // 3. 플레이어 컨트롤러를 가져와 Clear() 함수 호출
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.Clear(); // 애니메이션 재생, 조작 비활성화
        }

        // 4. 'clearDelay'초 뒤에 다음 씬 로드를 시작하는 코루틴(Coroutine) 실행
        StartCoroutine(LoadNextStageAfterDelay(clearDelay));
    }

    /// <summary>
    /// 지정된 시간만큼 기다린 후, 다음 스테이지를 계산하고 로드합니다.
    /// </summary>
    private IEnumerator LoadNextStageAfterDelay(float delay)
    {
        // 1. 설정된 시간(clearDelay)만큼 기다림
        yield return new WaitForSeconds(delay);

        // 2. 현재 씬의 이름(예: "Stage1")을 가져옴
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 3. 현재 씬 이름에서 스테이지 번호(숫자)를 추출
        int currentStageNum = 0;
        try
        {
            // "Stage"라는 글자를 지우고 숫자로 변환 (예: "Stage1" -> 1)
            // 씬 이름이 "Stage<번호>" 형식이 아니면 작동하지 않습니다.
            currentStageNum = int.Parse(currentSceneName.Replace("Stage", ""));
        }
        catch
        {
            // 씬 이름 형식이 잘못되었을 경우 오류 메시지 출력 후 중단
            Debug.LogError("씬 이름 형식이 잘못되었습니다! 'Stage<번호>' 형식이어야 합니다. (현재 씬: " + currentSceneName + ")");
            yield break; // 코루틴 중단
        }

        // 4. 다음 스테이지 계산 및 씬 이동
        if (currentStageNum == totalStages) // 마지막 스테이지(예: 4)를 클리어했다면
        {
            // (선택) 마지막 스테이지 정보를 저장
            PlayerPrefs.SetInt("CurrentStage", totalStages);
            PlayerPrefs.Save();
            
            // "에필로그" 씬으로 이동 (빌드 설정에 등록되어 있어야 함)
            SceneManager.LoadScene(epilogueSceneName);
        }
        else // 아직 1, 2, 3 스테이지라면
        {
            int nextStageNum = currentStageNum + 1;
            string nextSceneName = "Stage" + nextStageNum;

            // '이어하기'를 위해 다음 스테이지 번호를 PlayerPrefs에 저장
            PlayerPrefs.SetInt("CurrentStage", nextStageNum);
            PlayerPrefs.Save();

            // 다음 스테이지 씬으로 이동 (빌드 설정에 등록되어 있어야 함)
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
