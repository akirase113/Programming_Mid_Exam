using UnityEngine;
using UnityEngine.EventSystems; // 이벤트 시스템을 사용하기 위해 필수!

// 이 스크립트는 Collider 2D가 있어야만 작동합니다.
[RequireComponent(typeof(Collider2D))]
// IPointerClickHandler 인터페이스를 상속받습니다.
public class EnemyClickable : MonoBehaviour, IPointerClickHandler
{
    // 이 오브젝트가 클릭되었을 때 EventSystem이 자동으로 이 함수를 호출합니다.
    public void OnPointerClick(PointerEventData eventData)
    {
        // 1. "나 클릭당했어!" 라고 콘솔에 로그 출력 (디버깅용)
        Debug.Log(gameObject.name + " 가 클릭되었습니다!");

        // 2. 씬에 있는 Player 오브젝트를 찾습니다.
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 3. Player에게 붙어있는 PlayerAttack 스크립트를 찾습니다.
            PlayerAttack attackScript = player.GetComponent<PlayerAttack>();

            if (attackScript != null)
            {
                // 4. PlayerAttack 스크립트의 PerformAttack 함수를 호출합니다.
                //    공격 대상은 "나 자신(this.gameObject)"입니다.
                attackScript.PerformAttack(GetComponent<Collider2D>());
            }
            else
            {
                Debug.LogError("Player에서 PlayerAttack 스크립트를 찾지 못했습니다!");
            }
        }
        else
        {
            Debug.LogError("씬에서 'Player' 태그를 가진 오브젝트를 찾지 못했습니다!");
        }
    }
}