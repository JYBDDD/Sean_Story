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
        if (icon.sprite == null)  // 이미지가 없다면 알파값 0으로 조정
        {
            SetColor(0);
        }
        if (icon.sprite != null)  // 이미지가 있다면 알파값 1로 조정
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
        float currentTimeClick = 0;  // 클릭 이벤트
        currentTimeClick += Time.deltaTime;

        int ClickCount = eventData.clickCount;

        if (Items == null)
            return;

        if (currentTimeClick <= 0.75f && ClickCount >= 2 && Items.sprite != null)  // 0.75초안에 한번더누른다면
        {
            if (Items.ItemName.Contains(" 검 ")) // 무기라면
            {
                GameManager.Player.PlayerController.Stat.damage -= Items.ItemUseStrength;
                equipList.Remove(Items);
            }
            else  // 방어구라면
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

    // 더블클릭시 다시 인벤토리로 넣어주기
}
