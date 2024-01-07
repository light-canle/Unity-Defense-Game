using UnityEngine;
using TMPro;

public enum SystemType { Money = 0, Build }

public class SystemTextViewer : MonoBehaviour
{
    private TextMeshProUGUI textSystem;
    private TMPAlpha tmpAlpha;
    private void Awake()
    {
        textSystem = GetComponent<TextMeshProUGUI>();
        tmpAlpha = GetComponent<TMPAlpha>();
    }

    public void PrintText(SystemType type)
    {
        // �ý��� �ؽ�Ʈ�� ����Ѵ�.
        switch (type)
        {
            case SystemType.Money:
                textSystem.text = "System : Not enough money...";
                break;
            case SystemType.Build:
                textSystem.text = "System : Invaild build tower...";
                break;
        }
        // �ý��� �ؽ�Ʈ�� ��µ� �� ������ ������� �Ѵ�.
        tmpAlpha.FadeOut();
    }
}
