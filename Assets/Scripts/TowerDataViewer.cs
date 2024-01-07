using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image imageTower;                       // Ÿ���� �̹���
    [SerializeField]
    private TextMeshProUGUI textDamage;             // Ÿ���� �����
    [SerializeField]
    private TextMeshProUGUI textRate;               // Ÿ���� ���� ����
    [SerializeField]
    private TextMeshProUGUI textRange;              // Ÿ���� ���� ����
    [SerializeField]
    private TextMeshProUGUI textLevel;              // Ÿ���� ����
    [SerializeField]
    private TextMeshProUGUI textUpgradeCost;        // Ÿ���� ���׷��̵� ���
    [SerializeField]
    private TextMeshProUGUI textSellCost;           // Ÿ���� �Ǹ� ���
    [SerializeField]
    private TowerAttackRange towerAttackRange;      // Ÿ���� ���ݹ����� ǥ���ϱ� ���� ����
    [SerializeField]
    private Button buttonUpgrade;                   // ���׷��̵� ��ư�� ��� ����
    [SerializeField]
    private SystemTextViewer systemTextViewer;      // �ý��� �޽����� ����ϱ� ���� ����

    private TowerWeapon currentTower;
    private void Awake()
    {
        OffPanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    public void OnPanel(Transform towerWeapon)
    {
        // ����ؾ��ϴ� Ÿ�� ������ �޾ƿͼ� ����
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        // Ÿ�� ���� Panel On
        gameObject.SetActive(true);
        // Ÿ�� ���� ����
        UpdateTowerData();
        // Ÿ�� ������Ʈ �ֺ��� ǥ�õǴ� Ÿ�� ���ݹ��� Sprite On
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }

    public void OffPanel()
    {
        // Ÿ�� ���� Panel Off
        gameObject.SetActive(false);
        // Ÿ�� ���ݹ��� Sprite Off
        towerAttackRange.OffAttackRange();
    }

    private void UpdateTowerData()
    {
        if (currentTower.WeaponType == WeaponType.Cannon || currentTower.WeaponType == WeaponType.Laser)
        {
            // Ÿ���� �̹��� ũ��
            imageTower.rectTransform.sizeDelta = new Vector2(88, 59);
            // Ÿ���� ����� ���(������ ���� ���� ����������)
            textDamage.text = "Damage : " + currentTower.Damage
                            + "+" + "<color=red>" + currentTower.AddedDamage.ToString("F1") + "</color>";
        }
        else
        {
            // Ÿ���� �̹��� ũ��
            imageTower.rectTransform.sizeDelta = new Vector2(59, 59);
            // ���� Ÿ���� ��� - ����� ��� ���ӷ��� �����
            if (currentTower.WeaponType == WeaponType.Slow)
            {
                // Ÿ���� ���ӷ� ���
                textDamage.text = "Slow : " + currentTower.Slow * 100 + "%";
            }
            // ���� Ÿ���� ��� - ����� ��� ���ݷ� �������� �����
            else if (currentTower.WeaponType == WeaponType.Buff)
            {
                // ���ݷ� ������ ���
                textDamage.text = "Buff : " + currentTower.Buff * 100 + "%";
            }
            
        }
        // Ÿ���� �̹��� UI
        imageTower.sprite = currentTower.TowerSprite;
        // Ÿ���� ������ �������� �ؽ�Ʈ�� ǥ��
        textRate.text = "Rate : " + currentTower.Rate;
        textRange.text = "Range : " + currentTower.Range;
        textLevel.text = "Level : " + currentTower.Level;
        textUpgradeCost.text = currentTower.UpgradeCost.ToString();
        textSellCost.text = currentTower.SellCost.ToString();

        // ���׷��̵尡 �Ұ����ϸ�(�ִ� ������ ��) ��ư ��Ȱ��ȭ
        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;
    }

    public void OnClickEventTowerUpgrade()
    {
        // Ÿ�� ���׷��̵� �õ� (���� : true, ���� : false)
        bool isSuccess = currentTower.Upgrade();

        if ( isSuccess == true )
        {
            // Ÿ���� ���׷��̵� �Ǿ��� ������ Ÿ�� ���� ����
            UpdateTowerData();
            // Ÿ�� �ֺ��� ���̴� ���ݹ����� ����
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        }
        else
        {
            // Ÿ�� ���׷��̵� ����� �����ϴٰ� ���
            systemTextViewer.PrintText(SystemType.Money);
        }
    }

    public void OnClickEventTowerSell()
    {
        // Ÿ�� �Ǹ�
        currentTower.Sell();
        // ������ Ÿ���� ��������Ƿ� Panel, ���ݹ����� Off�Ѵ�.
        OffPanel();
    }
}
