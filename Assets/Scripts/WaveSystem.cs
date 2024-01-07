using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;               // 현재 스테이지의 모든 웨이브 정보
    [SerializeField]
    private EnemySpawner enemySpawner;  // 적을 생성하기 위한 변수
    private int currentWaveIndex = -1;  // 현재 웨이브 인덱스

    // 웨이브 정보 출력을 위한 Get 프로퍼티 (현재 웨이브, 총 웨이브)
    public int CurrentWave => currentWaveIndex + 1;     // 시작 인덱스가 0이므로 1을 더해야 함
    public int MaxWave => waves.Length;                 // 총 웨이브

    public void StartWave()
    {
        // 현재 맵에 적이 없고, Wave가 남아 있는 경우
        if (enemySpawner.EnemyList.Count == 0 && currentWaveIndex < waves.Length - 1)
        {
            // 인덱스의 시작이 -1이기 때문에 웨이브 인덱스 증가를 먼저 한다.
            currentWaveIndex++;
            // EnemySpawner의 StartWave() 함수 호출, 현재 웨이브 정보 제공
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }
    }
}

[System.Serializable]
public class Wave
{
    public float spawnTime;             // 현재 웨이브 적 생성 주기
    public int maxEnemyCount;           // 현재 웨이브 적 등장 숫자
    public GameObject[] enemyPrefabs;   // 현재 웨이브 적 등장 종류
}