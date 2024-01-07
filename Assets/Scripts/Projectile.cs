using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Movement2D movement2D;
    private Transform target;
    private float damage;

    public void SetUp(Transform target, float damage)
    {
        movement2D = GetComponent<Movement2D>();
        this.target = target;   // 타워가 설정해준 target
        this.damage = damage;   // 타워가 설정해준 damage
    }

    // Update is called once per frame
    private void Update()
    {
        if (target != null) // 타겟이 존재하는 경우
        {
            // 발사체를 target의 위치로 이동
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else // 타겟이 사라진 경우
        {
            // 발사체 오브젝트 삭제
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( !collision.CompareTag("Enemy") ) return;               // 적이 아닌 대상과 부딪힌 경우
        if (collision.transform != target) return;                  // 현재 target이 아닌 적과 부딪힌 경우

        collision.GetComponent<EnemyHP>().TakeDamage(damage);       // 적에게 대미지를 입힘
        Destroy(gameObject);                                        // 발사체 오브젝트를 삭제함
    }
}
