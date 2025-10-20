using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;          // ���� �̵� �ӵ�
    public string direction = "left";     // ���� �̵� ���� (left, right)
    public float range = 2.0f;          // �����̴� ����
    private Vector3 defPos;             // ���� ��ġ

    // Rigidbody2D ������Ʈ�� ĳ���� ����
    private Rigidbody2D rbody;

    private void Awake()
    {
        // GetComponent�� ������ ���� Awake�� Start���� �� ���� ȣ���մϴ�.
        rbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (direction == "right")
        {
            transform.localScale = new Vector2(-1, 1); // �������� �ٶ󺸵��� ����
        }
        else
        {
            transform.localScale = new Vector2(1, 1); // ������ �ٶ󺸵��� ����
        }
        defPos = transform.position; // ���� ��ġ ����
    }

    private void Update()
    {
        if (range > 0.0f)
        {
            // ���� ������ "left"�� ���� ���� �� üũ�� �ϵ��� ���� (���� ����)
            if (transform.position.x < defPos.x - (range / 2) && direction == "left")
            {
                Flip(); // �ߺ� �ڵ带 Flip() �Լ��� ��ü
            }
            // ���� ������ "right"�� ���� ������ �� üũ�� �ϵ��� ���� (���� ����)
            else if (transform.position.x > defPos.x + (range / 2) && direction == "right")
            {
                Flip(); // �ߺ� �ڵ带 Flip() �Լ��� ��ü
            }
        }
    }

    private void FixedUpdate()
    {
        // ĳ�õ� rbody ���� ���
        if (direction == "right")
        {
            // Rigidbody2D�� �Ӽ��� velocity �Դϴ�. (linearVelocity �ƴ�)
            rbody.velocity = new Vector2(speed, rbody.velocity.y); // ���������� �̵�
        }
        else
        {
            rbody.velocity = new Vector2(-speed, rbody.velocity.y); // �������� �̵�
        }
    }

    // ���� ��ȯ ������ ���� �Լ��� �и� (�ڵ� �ߺ� ����)
    private void Flip()
    {
        if (direction == "right")
        {
            direction = "left";
            transform.localScale = new Vector2(1, 1); // ������ �ٶ󺸵��� ����
        }
        else
        {
            direction = "right";
            transform.localScale = new Vector2(-1, 1); // �������� �ٶ󺸵��� ����
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // "Player" �±׿� �浹���� ���� ������ ��ȯ (���� �浹 ����)
        if (collision.CompareTag("Player"))
        {
            Flip();
        }
    }
}