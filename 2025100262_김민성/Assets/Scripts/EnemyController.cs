using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;          // ���� �̵� �ӵ�
    public string direction = "left";   // ���� �̵� ���� (left, right)
    public float range = 2.0f;          // �����̴� ����
    private Vector3 defPos;             // ���� ��ġ

    private void Start()
    {
        if (direction == "right")
        {
            transform.localScale = new Vector2(-1, 1);      // �������� �ٶ󺸵��� ����
        }
        else
        {
            transform.localScale = new Vector2(1, 1);       // ������ �ٶ󺸵��� ����
        }
        defPos = transform.position;    // ���� ��ġ ����
    }

    private void Update()
    {
        if (range > 0.0f)
        {
            // range / 2 �� �ؼ� ������ġ���� �¿�� ���ݾ� ������ ����
            if (transform.position.x < defPos.x - (range / 2))      // ���� ���� �����ߴ��� Ȯ��
            {
                direction = "right";
                transform.localScale = new Vector2(-1, 1);      // �������� �ٶ󺸵��� ����
            }
            else if (transform.position.x > defPos.x + (range / 2)) // ������ ���� �����ߴ��� Ȯ��
            {
                direction = "left";
                transform.localScale = new Vector2(1, 1);       // ������ �ٶ󺸵��� ����
            }
        }
    }

    private void FixedUpdate()
    {
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();        // Rigidbody2D ������Ʈ ��������

        if (direction == "right")
        {
            rbody.linearVelocity = new Vector2(speed, rbody.linearVelocity.y);  // ���������� �̵�
        }
        else
        {
            rbody.linearVelocity = new Vector2(-speed, rbody.linearVelocity.y); // �������� �̵�
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹 �� �̵� ���� ��ȯ
        if (direction == "right")
        {
            direction = "left";
            transform.localScale = new Vector2(1, 1);       // ������ �ٶ󺸵��� ����
        }
        else
        {
            direction = "right";
            transform.localScale = new Vector2(-1, 1);      // �������� �ٶ󺸵��� ����
        }
    }
}
