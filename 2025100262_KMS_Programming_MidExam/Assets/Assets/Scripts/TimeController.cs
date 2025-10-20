using UnityEngine;

public class TimeController : MonoBehaviour
{
    public bool isCountDown = true;     // true = 카운트 다운으로 시간 측정
    public float gameTime = 0.0f;       // 게임의 최대 시간
    public bool isTimeOver = false;     // true = 타이머 정지
    public float displayTime = 0.0f;    // 표시 시간

    private float times = 0.0f;         // 현재 시간

    private void Start()
    {
        if (isCountDown)
        {
            // 카운트 다운
            displayTime = gameTime;
        }
    }

    private void Update()
    {
        if (isTimeOver == false)
        {
            // time.deltaTime : 이전 프레임이 끝나고 현재 프레임이 시작될 때까지의 시간(초)
            times += Time.deltaTime;
            if (isCountDown)
            {
                // 카운트 다운
                displayTime = gameTime - times;
                if (displayTime <= 0.0f)
                {
                    displayTime = 0.0f;
                    isTimeOver = true;
                }
            }
            else
            {
                // 카운트 업
                displayTime = times;
                if (displayTime >= gameTime)
                {
                    displayTime = gameTime;
                    isTimeOver = true;
                }
            }
            //Debug.Log("TIMES : " + displayTime);
        }
    }
}
