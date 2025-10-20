using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float moveX = 0.0f;       // X�� �̵� �Ÿ�
    public float moveY = 0.0f;       // Y�� �̵� �Ÿ�
    public float times = 0.0f;      // �����̴� �ð�
    public float weight = 0.0f;     // ��� �ð�
    public bool isMoveWhenOn = false; // �÷��̾ �ö��� �� �̵� ����

    public bool isCanMove = true;
    private float perDX;        // 1�����Ӵ� X�̵� ��
    private float perDY;        // 1�����Ӵ� Y�̵� ��
    Vector3 defPos;      // �ʱ� ��ġ
    bool isReverse = false;  // �̵� ���� �÷���

    private void Start()
    {
        defPos = transform.position;            // �ʱ���ġ ����
        float timestep = Time.fixedDeltaTime;   // 1������ �ð� ���� (�⺻�� 0.02��)
        perDX = moveX / (times / timestep * times); // 1�����Ӵ� X�̵� �� ���
        perDY = moveY / (times / timestep * times); // 1�����Ӵ� Y�̵� �� ���

        if (isMoveWhenOn)
        {
            isCanMove = false; // �÷��̾ �ö��� �� �̵��ϴ� ��� �ʱ⿡�� �̵����� �ʵ��� ����
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(transform); // �÷��̾ �̵� ����� �ڽ����� �����Ͽ� �Բ� �̵��ϵ��� ��
        }
        if (isMoveWhenOn)
        {
            isCanMove = true; // �÷��̾ �ö��� �� �̵� ����
        }
        Debug.Log("�̵���Ͽ� ĳ���Ͱ� �ö�");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // �÷��̾ �̵� ����� �ڽ��� �ƴϰ� �����Ͽ� ����̵��� ������� �ʵ��� ��
            collision.transform.SetParent(null);
        }
        Debug.Log("�̵���Ͽ��� ĳ���Ͱ� ������");
    }

    private void FixedUpdate()
    {
        if (isCanMove)
        {
            float x = transform.position.x;     // ���� ��ġ�� X��ǥ
            float y = transform.position.y;     // ���� ��ġ�� Y��ǥ
            bool endX = false;        // X�� �̵� ���� �÷���
            bool endY = false;        // Y�� �̵� ���� �÷���

            if (isReverse)
            {// ������ �̵�
                // �̵����� ����� �̵� ��ġ�� �ʱ� ��ġ���� �۰ų�
                // �̵����� ������ �̵� ��ġ�� �ʱ� + �̵��Ÿ� ���� ū ���
                if ((perDX >= 0.0f && x <= defPos.x) || (perDX < 0.0f && x >= defPos.x))
                {
                    endX = true;    // X�� �̵� ����
                }
                if ((perDY >= 0.0f && y <= defPos.y) || (perDY < 0.0f && y >= defPos.y))
                {
                    endY = true;    // Y�� �̵� ����
                }

                // ��� �̵�
                Vector3 v = new Vector3(-perDX, -perDY, defPos.z);
                transform.Translate(v);
            }
            else
            {// ������ �̵�
                // �̵����� ����� �̵� ��ġ�� �ʱ� ��ġ���� ũ�ų�
                // �̵����� ������ �̵� ��ġ�� �ʱ� + �̵��Ÿ� ���� ���� ���
                if ((perDX >= 0.0f && x >= defPos.x + moveX) || (perDX < 0.0f && x <= defPos.x + moveX))
                {
                    endX = true;    // X�� �̵� ����
                }
                if ((perDY >= 0.0f && y >= defPos.y + moveY) || (perDY < 0.0f && y <= defPos.y + moveY))
                {
                    endY = true;    // Y�� �̵� ����
                }

                // ��� �̵�
                Vector3 v = new Vector3(perDX, perDY, defPos.z);
                transform.Translate(v);
            }

            if (endX && endY)
            {
                if (isReverse)  // ���������� �̵����̿��ٸ�
                {
                    transform.position = defPos; // �ʱ� ��ġ�� �̵�
                }

                isReverse = !isReverse; // �̵� ���� ��ȯ
                isCanMove = false;      // �̵� ����
                if (isMoveWhenOn == false)      // �÷��̾ �ö��� �� �̵��ϴ� ��찡 �ƴ϶��
                {
                    Invoke("Move", weight);         // weight ��ŭ ��� �� �̵� �簳
                }
            }
        }
    }

    public void Move()
    {
        isCanMove = true;
    }

    public void Stop()
    {
        isCanMove = false;
    }
}
