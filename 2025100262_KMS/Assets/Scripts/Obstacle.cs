using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // 이 오브젝트의 Trigger Collider(Is Trigger가 켜진) 안에 
    // 다른 Collider가 들어오는 순간 1회 호출됩니다.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 들어온 오브젝트의 태그가 "Player"인지 확인합니다.
        if (other.CompareTag("Player"))
        {
            // 2. 만약 플레이어가 맞다면, 플레이어의 PlayerController 스크립트를 가져옵니다.
            PlayerController player = other.GetComponent<PlayerController>();

            // 3. 스크립트를 성공적으로 가져왔는지 확인합니다 (null 체크)
            if (player != null)
            {
                // 4. 2단계에서 만든 플레이어의 Die() 함수를 호출합니다.
                player.Die();
            }
        }
    }
}