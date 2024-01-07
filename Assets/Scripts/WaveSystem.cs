using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;               // ���� ���������� ��� ���̺� ����
    [SerializeField]
    private EnemySpawner enemySpawner;  // ���� �����ϱ� ���� ����
    private int currentWaveIndex = -1;  // ���� ���̺� �ε���

    // ���̺� ���� ����� ���� Get ������Ƽ (���� ���̺�, �� ���̺�)
    public int CurrentWave => currentWaveIndex + 1;     // ���� �ε����� 0�̹Ƿ� 1�� ���ؾ� ��
    public int MaxWave => waves.Length;                 // �� ���̺�

    public void StartWave()
    {
        // ���� �ʿ� ���� ����, Wave�� ���� �ִ� ���
        if (enemySpawner.EnemyList.Count == 0 && currentWaveIndex < waves.Length - 1)
        {
            // �ε����� ������ -1�̱� ������ ���̺� �ε��� ������ ���� �Ѵ�.
            currentWaveIndex++;
            // EnemySpawner�� StartWave() �Լ� ȣ��, ���� ���̺� ���� ����
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }
    }
}

[System.Serializable]
public class Wave
{
    public float spawnTime;             // ���� ���̺� �� ���� �ֱ�
    public int maxEnemyCount;           // ���� ���̺� �� ���� ����
    public GameObject[] enemyPrefabs;   // ���� ���̺� �� ���� ����
}