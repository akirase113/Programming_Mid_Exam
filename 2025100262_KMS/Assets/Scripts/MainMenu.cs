using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필수!

public class MainMenu : MonoBehaviour
{
    // '게임 시작' 버튼이 호출할 함수
    public void OnStartButton()
    {
        // 데이터를 초기화 (예: 저장된 스테이지를 1로 리셋)
        PlayerPrefs.SetInt("CurrentStage", 1); 
        // 프롤로그 씬(대화 씬)으로 이동
        SceneManager.LoadScene("PrologueScene");
    }

    // '이어하기' 버튼이 호출할 함수
    public void OnContinueButton()
    {
        // PlayerPrefs에서 마지막으로 저장된 스테이지 번호를 불러옴
        // (저장된 값이 없으면 기본값 1을 불러옴)
        int lastStage = PlayerPrefs.GetInt("CurrentStage", 1);
        
        // 해당 스테이지 씬으로 바로 이동 (예: Stage1, Stage2...)
        SceneManager.LoadScene("Stage" + lastStage);
    }

    // '끝내기' 버튼이 호출할 함수
    public void OnQuitButton()
    {
        Debug.Log("게임 종료!"); // 유니티 에디터에서는 종료되지 않으므로 로그를 찍음
        Application.Quit(); // 실제 빌드된 게임에서는 여기서 종료됨
    }
}