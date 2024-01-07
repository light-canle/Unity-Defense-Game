using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Movement2D movement2D;
    private Transform target;
    private float damage;

    public void SetUp(Transform target, float damage)
    {
        movement2D = GetComponent<Movement2D>();
        this.target = target;   // Ÿ���� �������� target
        this.damage = damage;   // Ÿ���� �������� damage
    }

    // Update is called once per frame
    private void Update()
    {
        if (target != null) // Ÿ���� �����ϴ� ���
        {
            // �߻�ü�� target�� ��ġ�� �̵�
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else // Ÿ���� ����� ���
        {
            // �߻�ü ������Ʈ ����
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( !collision.CompareTag("Enemy") ) return;               // ���� �ƴ� ���� �ε��� ���
        if (collision.transform != target) return;                  // ���� target�� �ƴ� ���� �ε��� ���

        collision.GetComponent<EnemyHP>().TakeDamage(damage);       // ������ ������� ����
        Destroy(gameObject);                                        // �߻�ü ������Ʈ�� ������
    }
}
