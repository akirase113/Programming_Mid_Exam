using UnityEngine;
using UnityEngine.EventSystems; // �̺�Ʈ �ý����� ����ϱ� ���� �ʼ�!

// �� ��ũ��Ʈ�� Collider 2D�� �־�߸� �۵��մϴ�.
[RequireComponent(typeof(Collider2D))]
// IPointerClickHandler �������̽��� ��ӹ޽��ϴ�.
public class EnemyClickable : MonoBehaviour, IPointerClickHandler
{
    // �� ������Ʈ�� Ŭ���Ǿ��� �� EventSystem�� �ڵ����� �� �Լ��� ȣ���մϴ�.
    public void OnPointerClick(PointerEventData eventData)
    {
        // 1. "�� Ŭ�����߾�!" ��� �ֿܼ� �α� ��� (������)
        Debug.Log(gameObject.name + " �� Ŭ���Ǿ����ϴ�!");

        // 2. ���� �ִ� Player ������Ʈ�� ã���ϴ�.
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 3. Player���� �پ��ִ� PlayerAttack ��ũ��Ʈ�� ã���ϴ�.
            PlayerAttack attackScript = player.GetComponent<PlayerAttack>();

            if (attackScript != null)
            {
                // 4. PlayerAttack ��ũ��Ʈ�� PerformAttack �Լ��� ȣ���մϴ�.
                //    ���� ����� "�� �ڽ�(this.gameObject)"�Դϴ�.
                attackScript.PerformAttack(GetComponent<Collider2D>());
            }
            else
            {
                Debug.LogError("Player���� PlayerAttack ��ũ��Ʈ�� ã�� ���߽��ϴ�!");
            }
        }
        else
        {
            Debug.LogError("������ 'Player' �±׸� ���� ������Ʈ�� ã�� ���߽��ϴ�!");
        }
    }
}