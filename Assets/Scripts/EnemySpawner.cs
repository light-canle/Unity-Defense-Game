using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyHPSliderPrefab;     // �� ü���� ��Ÿ���� Slider UI ������
    [SerializeField]
    private Transform canvasTransform;          // UI�� ǥ���ϴ� Canvas ������Ʈ�� Transform
    [SerializeField]
    private Transform[] wayPoints;              // ���� ���������� �̵� ���
    [SerializeField]
    private PlayerHP playerHP;                  // �÷��̾��� ü�� ������Ʈ
    [SerializeField]
    private PlayerGold playerGold;              // �÷��̾��� ��� ������Ʈ
    private Wave currentWave;                   // ���� ���̺� ����
    private int currentEnemyCount;              // ���� ���̺꿡 �����ִ� �� ����(���̺� ���۽� max�� ����, ���� ������ �� ���� -1)
    private List<Enemy> enemyList;              // ���� �ʿ� �����ϴ� ��� ���� ����

    // ���� ������ ������ EnemySpawner���� �ϱ� ������ set�� �ʿ� ����
    public List<Enemy> EnemyList => enemyList;
    // ���� ���̺��� �����ִ� ��, �ִ� �� ����
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;
    private void Awake()
    {
        // �� ����Ʈ �޸� �Ҵ�
        enemyList = new List<Enemy>();
        currentWave = new Wave();
        currentWave.maxEnemyCount = 0;
        currentEnemyCount = 0;
        
    }

    public void StartWave(Wave wave)
    {
        // �Ű������� �޾ƿ� ���̺� ���� ����
        currentWave = wave;
        // ���� ���̺��� �ִ� �� ���ڸ� ����
        currentEnemyCount = currentWave.maxEnemyCount;
        // ���� ���̺� ����
        StartCoroutine("SpawnEnemy");
    }
    private IEnumerator SpawnEnemy()
    {
        // ���� ���̺꿡�� ������ �� ����
        int spawnEnemyCount = 0;
        // ���� ���̺꿡�� �����Ǿ�� �ϴ� ���� ���ڸ�ŭ ���� �����ϰ� �ڷ�ƾ�� ������
        while (spawnEnemyCount < currentWave.maxEnemyCount)
        {
            // ���̺꿡 �����ϴ� ���� ������ ���� ������ ��, ������ ���� �����ϵ��� �ϰ�, �� ������Ʈ ����
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>();          // ��� ������ ���� enemy ������Ʈ
            // this�� �� �ڽ� ( �ڽ��� EnemySpawner ���� )
            enemy.SetUp(this, wayPoints);                       // wayPoint ������ �Ű������� SetUp() ȣ��
            enemyList.Add(enemy);                               // ����Ʈ�� ��� ������ �� ���� ����

            SpawnEnemyHPSlider(clone);                          // �� ü���� ��Ÿ���� Slider UI ���� �� ����
            // ���� ���̺꿡�� ������ ���� ���ڸ� 1 ������Ŵ
            spawnEnemyCount++;

            yield return new WaitForSeconds(currentWave.spawnTime);         // spawnTime �ð� ���� ���
        }
    }

    public void DestroyEnemy(EnemyDestoryType type, Enemy enemy, int gold)
    {
        // ���� ��ǥ�������� �������� ��
        if (type == EnemyDestoryType.Arrive)
        {
            // �÷��̾� ü�� -1
            playerHP.TakeDamage(1);
        }
        // ���� �߻�ü�� �°� ������ ���
        else if (type == EnemyDestoryType.Kill)
        {
            // ���� ������ ���� ��带 ȹ��
            playerGold.CurrentGold += gold;
        }
        // ���� ����� ������ ���� ���̺��� ���� �� ���� ����(UI ǥ�ÿ�)
        currentEnemyCount--;
        // ����Ʈ���� ����� �� ���� ����
        enemyList.Remove(enemy);
        // �� ������Ʈ ����
        Destroy(enemy.gameObject);
    }

    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        // �� ü���� ��Ÿ���� Slider UI ����
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        // Slider UI ������Ʈ�� Canvas ������Ʈ�� �ڽ����� ���� (�̷��� �ؾ� UI�� ȭ�鿡 ���δ�)
        sliderClone.transform.SetParent(canvasTransform);
        // ���� �������� �ٲ� ũ�⸦ �ٽ� (1, 1, 1)�� ����
        sliderClone.transform.localScale = Vector3.one;
        // Slider UI�� �Ѿƴٴ� ����� �������� ����
        sliderClone.GetComponent<SliderPositionAutoSetter>().SetUp(enemy.transform);
        // Slider UI�� �ڽ��� ü�� ������ ǥ���ϵ��� ����
        sliderClone.GetComponent<EnemyHPViewer>().SetUp(enemy.GetComponent<EnemyHP>());
    }
}
