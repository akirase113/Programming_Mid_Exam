using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject magicEffectPrefab;
    // Inspector에서 설정할 변수: 마법이 생성될 Y축 오프셋 (얼마나 위에서 시작할지)
    [Tooltip("마법 이펙트가 적의 발밑보다 얼마나 높은 곳에서 생성될지")]
    public float magicSpawnHeightOffset = 3f; 
    // Inspector에서 설정할 변수: 마법이 땅에 꽂히는 속도
    [Tooltip("마법 이펙트가 발밑으로 내려오는 속도")]
    public float magicFallSpeed = 10f; 

    // ... (Update, AttackCheck 함수는 그대로 둡니다) ...

    void PerformAttack(Collider2D enemyCollider)
    {
        Transform enemyTransform = enemyCollider.transform;

        // 1. "적의 발밑" 위치 계산 (이건 동일)
        float footOffsetY = enemyCollider.bounds.size.y / 2f;
        Vector2 targetFootPosition = new Vector2(enemyTransform.position.x, enemyTransform.position.y - footOffsetY);

        // 2. "적의 발밑 상공" 위치 계산 (새로운 시작 위치)
        // targetFootPosition.y에서 magicSpawnHeightOffset만큼 위로 올립니다.
        Vector2 startSpawnPosition = new Vector2(targetFootPosition.x, targetFootPosition.y + magicSpawnHeightOffset);

        // 3. '마법 이펙트 프리팹'을 '적의 발밑 상공' 위치에 생성
        if (magicEffectPrefab != null)
        {
            GameObject instantiatedMagic = Instantiate(magicEffectPrefab, startSpawnPosition, Quaternion.identity);
            
            // 4. 생성된 마법 이펙트에게 목표 위치와 속도를 알려주는 스크립트 추가
            //    이펙트 프리팹에 MagcEffectMover.cs를 붙여야 합니다!
            MagicEffectMover mover = instantiatedMagic.GetComponent<MagicEffectMover>();
            if(mover != null)
            {
                mover.SetTarget(targetFootPosition, magicFallSpeed);
            }
            else
            {
                Debug.LogError("MagicEffectMover 스크립트가 MagicEffectPrefab에 없습니다!");
            }
        }
        else
        {
            Debug.LogError("Magic Effect Prefab이 할당되지 않았습니다!");
        }

        // 5. 적에게 데미지 입히는 것은 동일
        Enemy enemyScript = enemyCollider.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage();
        }
    }
}