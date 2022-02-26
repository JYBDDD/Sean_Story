using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour, IPointerClickHandler
{
    private Image icon;

    public Item.ItemData itemData = new Item.ItemData();

    public Item.ItemData Items
    {
        get { return itemData; }
        set { itemData = value; }
    }

    public static List<Item.ItemData> equipList = new List<Item.ItemData>();

    private void Start()
    {
        icon = GetComponent<Image>();
    }

    private void Update()
    {
        if (icon.sprite == null)  // �̹����� ���ٸ� ���İ� 0���� ����
        {
            SetColor(0);
        }
        if (icon.sprite != null)  // �̹����� �ִٸ� ���İ� 1�� ����
        {
            SetColor(1);
        }

        if (Items.sprite != null)
            icon.sprite = itemData.sprite;
        if (Items.sprite == null)
            icon.sprite = null;
    }

    private void SetColor(float alpha)
    {
        Color color = icon.color;
        color.a = alpha;
        icon.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        float currentTimeClick = 0;  // Ŭ�� �̺�Ʈ
        currentTimeClick += Time.deltaTime;

        int ClickCount = eventData.clickCount;

        if (Items == null)
            return;

        if (currentTimeClick <= 0.75f && ClickCount >= 2 && Items.sprite != null)  // 0.75�ʾȿ� �ѹ��������ٸ�
        {
            if (Items.ItemName.Contains(" �� ")) // ������
            {
                GameManager.Player.PlayerController.Stat.damage -= Items.ItemUseStrength;
                equipList.Remove(Items);
            }
            else  // �����
            {
                GameManager.Player.PlayerController.Stat.maxHp -= Items.ItemUseStrength;
                equipList.Remove(Items);
            }

            Items.Amount += 1;
            GameManager.Inven.InsertItemInformation(gameObject.GetComponent<EquipSlot>().Items);
            Items.sprite = null;
            Items.ItemUseStrength = 0;
            icon.sprite = null;

        }
    }

    // ����Ŭ���� �ٽ� �κ��丮�� �־��ֱ�
}
