using UnityEngine;
using System.Collections;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[] towerTemplate;          // Ÿ�� ���� (���ݷ�, ���� �ӵ� ��)
    [SerializeField]    
    private EnemySpawner enemySpawner;              // ���� �ʿ� �����ϴ� �� ����Ʈ ������ ��� ���� ����
    [SerializeField]
    private PlayerGold playerGold;                  // �÷��̾ ������ �ִ� ���� ��� �ִ� ����
    [SerializeField]
    private SystemTextViewer systemTextViewer;      // �� ����, �Ǽ� �Ұ��� ���� �ý��� �޽��� ���
    private bool isOnTowerButton = false;           // Ÿ�� �Ǽ� ��ư�� �������� üũ
    private GameObject followTowerClone = null;     // �ӽ� Ÿ�� ��� �Ϸ� �� ������ ���� �����ϴ� ����
    private int towerType;                          // Ÿ�� �Ӽ�(����)

    public void ReadyToSpawnTower(int type)
    {
        towerType = type;

        // ��ư�� �ߺ��ؼ� ������ ���� �����ϱ� ���� �ʿ�
        if ( isOnTowerButton == true )
        {
            return;
        }
        
        // Ÿ�� �Ǽ� ���� ���� Ȯ��
        // 1. Ÿ���� �Ǽ��� ��ŭ ���� ������ Ÿ�� �Ǽ��� ���� ����
        if (towerTemplate[towerType].weapon[0].cost > playerGold.CurrentGold)
        {
            // ��尡 �����ؼ� Ÿ�� �Ǽ��� �Ұ����ϴٰ� �����
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        // Ÿ�� �Ǽ� ��ư�� �����ٰ� ����
        isOnTowerButton = true;
        // ���콺�� ����ٴϴ� �ӽ� Ÿ�� ����
        followTowerClone = Instantiate(towerTemplate[towerType].followTowerPrefab);
        // Ÿ�� �Ǽ��� ����� �� �ִ� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnTowerCancelSystem");
    }
    public void SpawnTower(Transform tileTransform)
    {
        // Ÿ�� �Ǽ� ��ư�� ������ ���� Ÿ�� �Ǽ� ����
        if (isOnTowerButton == false)
        {
            return;
        }

        Tile tile = tileTransform.GetComponent<Tile>();
        
        // 2. ���� Ÿ���� ��ġ�� �̹� Ÿ���� �Ǽ��Ǿ� ������ Ÿ�� �Ǽ��� ���� ����
        if (tile.IsBuildTower == true)
        {
            // ���� ��ġ�� Ÿ�� �Ǽ��� �Ұ����ϴٰ� ���
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }

        // �ٽ� Ÿ�� �Ǽ� ��ư�� ������ Ÿ���� �Ǽ��� �� �ֵ��� false�� �ٲ�
        isOnTowerButton = false;
        // Ÿ���� �Ǽ��Ǿ� �������� ����
        tile.IsBuildTower = true;
        // Ÿ�� �Ǽ��� �ʿ��� ��ŭ ��带 �Ҹ�
        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;
        // ������ Ÿ���� ��ġ�� Ÿ�� �Ǽ� (Ÿ�Ϻ��� z�� -1�� ��ġ�� ��ġ)
        Vector3 position = tileTransform.position + Vector3.back;
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity);
        // Ÿ�� ���⿡ �ڽŰ� enemySpawner, playerGold, tile ���� ����
        clone.GetComponent<TowerWeapon>().SetUp(this, enemySpawner, playerGold, tile);
        // ���� ��ġ�Ǵ� Ÿ���� ���� Ÿ�� �ֺ��� ��ġ�� ��� ���� ȿ���� ���� �� �ֵ��� ��� ���� Ÿ���� ���� ȿ�� ����
        OnBuffAllBuffTowers();
        // Ÿ���� ��ġ�߱� ������ ���콺�� ����ٴϴ� �ӽ� ��ü�� �����Ѵ�
        Destroy(followTowerClone);
        // Ÿ�� �Ǽ��� ����� �� �ִ� �ڷ�ƾ �Լ� ���� (�̹� �Ǽ��� �Ǿ����Ƿ�)
        StopCoroutine("OnTowerCancelSystem");
    }

    private IEnumerator OnTowerCancelSystem()
    {
        while (true)
        {
            // ESCŰ �Ǵ� ���콺 ������ ��ư�� ������ �� Ÿ�� �Ǽ� ���
            if ( Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1) )
            {
                isOnTowerButton = false;
                // ���콺�� ����ٴϴ� �ӽ� ��ü ����
                Destroy(followTowerClone);
                break;
            }
            yield return null;
        }
    }

    public void OnBuffAllBuffTowers()
    {
        // �ʿ� ��ġ�� ��� Ÿ���� ã�� ����
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        // �迭�� ��ȸ
        for (int i = 0; i < towers.Length; i++)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();
            // ���� ���� Ÿ���� ��� �Լ��� ȣ���� ���� ������ �����Ѵ�.
            if (weapon.WeaponType == WeaponType.Buff)
            {
                weapon.OnBuffAroundTower();
            }
        }
    }
}
