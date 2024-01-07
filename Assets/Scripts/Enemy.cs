using System.Collections;
using UnityEngine;

public enum EnemyDestoryType { Kill = 0, Arrive } // ���� ������ �޾� ����ߴ��� �� ������ �����ߴ����� �Ǵ��ϱ� ���� ������

public class Enemy : MonoBehaviour
{
    private int wayPointCount;          // �̵� ��� ����
    private Transform[] wayPoints;      // �̵� ��� ����
    private int currentIndex = 0;       // ���� ��ǥ���� �ε���
    private Movement2D movement2D;      // ������Ʈ �̵� ����
    private EnemySpawner enemySpawner;  // ���� ������ �ڽ��� �ϴ� ���� �ƴ϶� EnemySpawner�� �̿��ؼ� �����ϱ� ���� ������ ����
    [SerializeField]
    private int gold = 10;              // ���� óġ�� �� ���� �� �ִ� ���

    private bool isCollisionwithWall = false;      // ��� ��Ż ���� ������ ���� ���� - ���� EraseWall�� �浹�ߴ��� ��� ����
    public void SetUp(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        // �� �̵���� WayPoints ���� ����
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        // ���� ��ġ�� ù��° wayPoint ��ġ�� ����
        transform.position = wayPoints[currentIndex].position;

        // �� �̵�/��ǥ���� ���� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnMove");
    }
    private IEnumerator OnMove()
    {
        // ���� ���� �̵� ����
        NextMoveTo();

        while (true)
        {
            // �� ������Ʈ ȸ��
            transform.Rotate(Vector3.forward * 10);

            /*
             * ���� ������ġ�� ��ǥ��ġ�� �Ÿ��� 0.02 * movement2D.MoveSpeed���� ���� �� if ���ǹ��� ������
             */
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.03f * movement2D.MoveSpeed)
            {
                // ���� �̵� ���� ����
                NextMoveTo();
            }
            yield return null;
        }
    }

    private void NextMoveTo()
    {
        // ���� �̵��� wayPoints�� ���� �ִ� ���
        if (currentIndex < wayPointCount - 1)
        {
            // ���� ��ġ�� ��Ȯ�ϰ� ��ǥ ��ġ�� ����
            transform.position = wayPoints[currentIndex].position;
            // �̵� ���� ���� >> ���� ��ǥ�������� ������
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        // ���� ��ġ�� ������ wayPoint�� ���
        else
        {
            // ���� ��ǥ������ �����ؼ� �������� ��쿡�� ���� ���� ����
            gold = 0;
            // �� ������Ʈ ����
            OnDie(EnemyDestoryType.Arrive);
        }
    }

    public void OnDie(EnemyDestoryType type)
    {
        // EnemySpawner���� ����Ʈ�� �� ��ü���� �����ϱ� ������ �� ��ü���� ���� Destroy��
        // �ϴ� ���� �ƴ϶� �ʿ��� ó���� �ϵ��� �Ѵ�.
        enemySpawner.DestroyEnemy(type, this, gold);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� Ȯ���� �� ��ü�� ��θ� ����� ������ �־� ���� �Ѿ�� �� ������Ʈ�� ������Ŵ
        // ������ ���� ���̹Ƿ� �߻�ü�� �¾� ������ ������ ����

        // �� �̺�Ʈ�� ������ �߻��� �� �����Ƿ� 1���� ����ǰ� �ϱ� ���� �� if���� ����.
        if (isCollisionwithWall == true)
        {
            return;
        }

        if (collision.CompareTag("EraseWall"))
        {
            isCollisionwithWall = true;
            // �� ������Ʈ ����
            OnDie(EnemyDestoryType.Kill);
        }
    }
}
