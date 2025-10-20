using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject objPrefab;            // ���뿡�� �߻�Ǵ� ��ź ������
    public float delayTime = 3.0f;         // ��ź �߻� ���� �ð�
    public float fireSpeedX = -4.0f;      // ��ź�� X�࿡ �������� ��
    public float fireSpeedY = 0.0f;       // ��ź�� Y�࿡ �������� ��
    public float length = 8.0f;         // �÷��̾� ���� �Ÿ�

    GameObject player;            // �÷��̾� ������Ʈ
    GameObject gateObj;            // ���� �Ա� ������Ʈ
    float passedTime = 0.0f;         // ��� �ð�

    public AudioClip soundCannon; // ���� �߻� ����

    private void Start()
    {
        Transform tr = transform.Find("gate");      // �ڽ��� �ڽ� ������Ʈ �� "gate" �̸��� ���� ������Ʈ Ž��
        gateObj = tr.gameObject;        // ���� �Ա� ������Ʈ ����
        player = GameObject.FindGameObjectWithTag("Player");   // �±װ� "Player"�� ������Ʈ Ž��
    }

    private void Update()
    {
        passedTime += Time.deltaTime;       // ��� �ð� ����
        if (CheckLength(player.transform.position))     // �÷��̾ ���� �Ÿ� �̳��� ������
        {
            if (passedTime >= delayTime)    // delayTime�� ���������
            {
                passedTime = 0.0f;          // ��� �ð� �ʱ�ȭ
                // ���� �Ա� ��ġ ���
                Vector3 pos = new Vector3(gateObj.transform.position.x, gateObj.transform.position.y, transform.position.z);

                GameObject obj = Instantiate(objPrefab, pos, Quaternion.identity);  // ��ź ������Ʈ ����

                Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();        // ��ź�� Rigidbody2D ������Ʈ ���
                Vector2 v = new Vector2(fireSpeedX, fireSpeedY);    // ��ź�� ���� �� ���ͷ� ����
                rbody.AddForce(v, ForceMode2D.Impulse);         // ��ź�� �� �ֱ�

                AudioSource src = obj.AddComponent<AudioSource>(); // AudioSource ������Ʈ �߰�
                src.clip = soundCannon;        // ����� Ŭ�� �ε�
                src.Play();
            }
        }
    }

    private bool CheckLength(Vector2 targetPos)
    {
        bool ret = false;
        float d = Vector2.Distance(transform.position, targetPos);   // ������ �÷��̾� ���� �Ÿ� ���
        if (length >= d)
        {
            ret = true;    // �÷��̾ ���� �Ÿ� �̳��� ������ true ��ȯ
        }
        return ret;
    }
}
