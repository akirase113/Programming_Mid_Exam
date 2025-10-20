using UnityEngine;
using UnityEngine.SceneManagement;  // 씬을 변경하기 위한 네임스페이스

public class ChangeScene : MonoBehaviour
{
    public string sceneName;        // 불러올 씬 이름

    // 씬 불러오기 함수
    public void Load()
    {
        SceneManager.LoadScene(sceneName); // sceneName에 지정된 씬 불러오기
    }
}
