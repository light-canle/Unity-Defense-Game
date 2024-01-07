using System.Collections;
using UnityEngine;

public enum EnemyDestoryType { Kill = 0, Arrive } // 적이 공격을 받아 사망했는지 끝 지점에 도달했는지를 판단하기 위한 열거형

public class Enemy : MonoBehaviour
{
    private int wayPointCount;          // 이동 경로 개수
    private Transform[] wayPoints;      // 이동 경로 정보
    private int currentIndex = 0;       // 현재 목표지점 인덱스
    private Movement2D movement2D;      // 오브젝트 이동 제어
    private EnemySpawner enemySpawner;  // 적의 삭제를 자신이 하는 것이 아니라 EnemySpawner를 이용해서 삭제하기 위해 정의한 변수
    [SerializeField]
    private int gold = 10;              // 적을 처치할 시 얻을 수 있는 골드

    private bool isCollisionwithWall = false;      // 경로 이탈 오류 때문에 넣은 변수 - 적이 EraseWall과 충돌했는지 담는 변수
    public void SetUp(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        // 적 이동경로 WayPoints 정보 설정
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        // 적의 위치를 첫번째 wayPoint 위치로 설정
        transform.position = wayPoints[currentIndex].position;

        // 적 이동/목표지점 설정 코루틴 함수 시작
        StartCoroutine("OnMove");
    }
    private IEnumerator OnMove()
    {
        // 다음 방향 이동 설정
        NextMoveTo();

        while (true)
        {
            // 적 오브젝트 회전
            transform.Rotate(Vector3.forward * 10);

            /*
             * 적의 현재위치와 목표위치의 거리가 0.02 * movement2D.MoveSpeed보다 작을 때 if 조건문을 실행함
             */
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.03f * movement2D.MoveSpeed)
            {
                // 다음 이동 방향 설정
                NextMoveTo();
            }
            yield return null;
        }
    }

    private void NextMoveTo()
    {
        // 아직 이동할 wayPoints가 남아 있는 경우
        if (currentIndex < wayPointCount - 1)
        {
            // 적의 위치를 정확하게 목표 위치로 설정
            transform.position = wayPoints[currentIndex].position;
            // 이동 방향 설정 >> 다음 목표지점으로 설정함
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        // 현재 위치가 마지막 wayPoint인 경우
        else
        {
            // 적이 목표지점에 도달해서 없어지는 경우에는 돈을 주지 않음
            gold = 0;
            // 적 오브젝트 삭제
            OnDie(EnemyDestoryType.Arrive);
        }
    }

    public void OnDie(EnemyDestoryType type)
    {
        // EnemySpawner에서 리스트로 적 개체들을 관리하기 때문에 적 객체에서 직접 Destroy를
        // 하는 것이 아니라 필요한 처리를 하도록 한다.
        enemySpawner.DestroyEnemy(type, this, gold);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 낮은 확률로 적 객체가 경로를 벗어나는 오류가 있어 벽을 넘어가면 적 오브젝트를 삭제시킴
        // 오류로 인한 것이므로 발사체에 맞아 쓰러진 것으로 판정

        // 이 이벤트가 여러번 발생할 수 있으므로 1번만 실행되게 하기 위해 이 if문을 쓴다.
        if (isCollisionwithWall == true)
        {
            return;
        }

        if (collision.CompareTag("EraseWall"))
        {
            isCollisionwithWall = true;
            // 적 오브젝트 삭제
            OnDie(EnemyDestoryType.Kill);
        }
    }
}
