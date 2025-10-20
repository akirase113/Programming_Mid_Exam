#if UNITY_EDITOR    // 유니티 에디터에서만 컴파일되도록 설정
using UnityEditor;
using UnityEngine;

public class ScoreReset
{
    [MenuItem("Tools/PlayerPrefs 초기화")]
    public static void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
#endif