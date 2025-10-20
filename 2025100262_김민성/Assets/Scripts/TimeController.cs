using UnityEngine;

public class TimeController : MonoBehaviour
{
    public bool isCountDown = true;     // true = ī��Ʈ �ٿ����� �ð� ����
    public float gameTime = 0.0f;       // ������ �ִ� �ð�
    public bool isTimeOver = false;     // true = Ÿ�̸� ����
    public float displayTime = 0.0f;    // ǥ�� �ð�

    private float times = 0.0f;         // ���� �ð�

    private void Start()
    {
        if (isCountDown)
        {
            // ī��Ʈ �ٿ�
            displayTime = gameTime;
        }
    }

    private void Update()
    {
        if (isTimeOver == false)
        {
            // time.deltaTime : ���� �������� ������ ���� �������� ���۵� �������� �ð�(��)
            times += Time.deltaTime;
            if (isCountDown)
            {
                // ī��Ʈ �ٿ�
                displayTime = gameTime - times;
                if (displayTime <= 0.0f)
                {
                    displayTime = 0.0f;
                    isTimeOver = true;
                }
            }
            else
            {
                // ī��Ʈ ��
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
