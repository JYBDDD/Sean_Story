using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class InventoryDragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    [SerializeField]
    private Image itemBlankContainer;
    private Text ContainerText;

    public static Slot[] slot;  // ���Դ�������

    private Slot StartDragSlot;  // ���� �巡������ �ӽ�����
    private Slot EndDragSlot;    // �� �巡������ �ӽ�����
    private bool CheckStartBool = false;  // ��ĭ�� �巡���Ѵٸ� true

    // ������ ���� ����
    [Space]
    [SerializeField]
    private Image itemTooltipBackImg;
    [SerializeField]
    private Image itemTooltipImg;
    [SerializeField]
    private Text itemName;
    [SerializeField]
    private Text itemTooltipText;
    [Space]

    // ����â ��� ����
    [SerializeField]
    private GameObject Casque_G;
    [SerializeField]
    private GameObject Armor_G;
    [SerializeField]
    private GameObject Leggings_G;
    [SerializeField]
    private GameObject Weapon_G;
    [SerializeField]
    private GameObject Gloves_G;
    [SerializeField]
    private GameObject Boots_G;

    //saveData��
    public static List<Slot> InvenList = new List<Slot>();
    public static bool InvenLoadBool = false;

    public void Awake()
    {
        slot = GetComponentsInChildren<Slot>();   // InventorySet�� [DefaultExecutionOrder(-2)] �� �־� ���� ���Ե��� ���������ְ� �״��� ���԰��� ã���� ������
    }

    public void OnEnable()  
    {
        itemTooltipBackImg.gameObject.SetActive(false);
        ContainerText = itemBlankContainer.GetComponentInChildren<Text>();
    }

    public void Start()
    {
        if (slot != null)  // slot ������ ���� ����
        {
            if (InvenLoadBool == true)
            {
                ItemListClear();
                GameManager.Json.InvenLoadData("SaveData");
                InvenLoadBool = false;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CheckStartBool = false;
        var result = eventData.pointerCurrentRaycast;

        if(result.gameObject.GetComponentInChildren<Slot>().Items.sprite == null | result.gameObject.GetComponentInChildren<Slot>() == null)
        {
            CheckStartBool = true;
            itemBlankContainer.gameObject.SetActive(false);
            itemBlankContainer.GetComponent<Slot>().Items = null;
            return;
        }

        for (int i = 0; i < 24; i++)
        {
            if (result.gameObject.GetComponentInChildren<Slot>().Items == InventoryManager.slotList[i].Items)
            {
                itemBlankContainer.gameObject.SetActive(true);
                StartDragSlot = slot[i];
                itemBlankContainer.GetComponent<Slot>().Items = StartDragSlot.Items;
                itemBlankContainer.sprite = StartDragSlot.Items.sprite;

                if (StartDragSlot.Items.ItemTypes == Define.ItemType.Equipment)
                    ContainerText.text = "";
                if (StartDragSlot.Items.ItemTypes != Define.ItemType.Equipment)
                    ContainerText.text = $"X{StartDragSlot.Items.Amount}";
            }
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        itemBlankContainer.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (StartDragSlot == null | CheckStartBool == true) 
        {
            itemBlankContainer.gameObject.SetActive(false);
            itemBlankContainer.GetComponent<Slot>().Items = null;
            return;
        }

        var result = eventData.pointerCurrentRaycast;
        if (result.gameObject == null)  // �������� ���ٸ� ����
        {
            itemBlankContainer.gameObject.SetActive(false);
            itemBlankContainer.GetComponent<Slot>().Items = null;
            return;
        }


        var consumptionSlot = result.gameObject.GetComponent<ConsumptionSlot>();
        if (consumptionSlot != null)   // ���� ���°��� �Һ񽽷��̶��      (�Һ� ���� �����ϴ� ����)
        {
            Item.ItemData consumItem = new Item.ItemData();
            consumItem = StartDragSlot.Items;  // ������ StartDragSlot �� Slot[i]�� �ް� �ֱ� ������ Items ���� ����Ǿ� �Һ񽽷Կ��� ������ ���� �κ��丮 �ȿ��ִ� �����۵� ���� �Ҹ� �ȴ�

            if (consumItem.ItemTypes == Define.ItemType.Consumption)
            {
                if (consumptionSlot.Items != consumItem && consumptionSlot.Items.sprite != null)
                {
                    if (ConsumptionSlot.consumList[0].ItemName != consumItem.ItemName)  // 1��° ���Ը���Ʈ
                        ConsumptionSlot.consumList.RemoveAt(0);

                    if(ConsumptionSlot.consumList.Count > 1)
                        if (ConsumptionSlot.consumList[1].ItemName != consumItem.ItemName)  // 2��° ���Ը���Ʈ
                            ConsumptionSlot.consumList.RemoveAt(1);
                }
                    
                consumptionSlot.Items = consumItem;
                ConsumptionSlot.consumList.Add(consumItem);
                if (ConsumptionSlot.consumList.Count > 2)  // ������ 3���� �ɶ� ������ ����Ʈ ����
                    ConsumptionSlot.consumList.RemoveAt(2);
                
            }

            itemBlankContainer.gameObject.SetActive(false);
            itemBlankContainer.GetComponent<Slot>().Items = null;
            return;

        }


        EndDragSlot = result.gameObject.GetComponentInChildren<Slot>();  /// 1.  ����ְų� ����ִ� Slot���� �����´�
        Item.ItemData item = EndDragSlot.Items;  // �����۰� �ӽ� ����

        for (int i = 0; i < 24; i++)
        {
            if (StartDragSlot == slot[i])  // 2 ~ 3  ó�� ���۽� �巡���� ���̶�� ó�� ���� �巡�׽��Կ� ������ ���Ե����͸� ������ �ִ´�
            {
                slot[i].Items = item;
            }
            if (EndDragSlot == slot[i])  // 2 ~ 3  ������ ���Ե����Ͷ�� �� �����Ϳ� ���۽õ巡���� �����Ͱ��� �ִ´�
            {
                slot[i].Items = itemBlankContainer.GetComponent<Slot>().Items; 
            }

            itemBlankContainer.gameObject.SetActive(false);
        }
        itemBlankContainer.GetComponent<Slot>().Items = null;
        
    }

    public void OnPointerEnter(PointerEventData eventData) // ������ ����
    {
        var result = eventData.pointerCurrentRaycast;
        if (result.gameObject.GetComponentInChildren<Slot>().Items == null)  // ������ �����Ͱ� Null�̶�� ����
        {
            itemTooltipBackImg.gameObject.SetActive(false);
            return;
        }

        if(result.gameObject.GetComponentInChildren<Slot>().Items.sprite == null)  // ������ �̹����� Null �̶�� ����  (���������� ���� ���� ���Ͻ�����)
        {
            itemTooltipBackImg.gameObject.SetActive(false);
            return;
        }

        for (int i = 0; i < 24; i++)
        {
            if(slot[i].Items != null)
            {
                if (result.gameObject.GetComponentInChildren<Slot>().Items.sprite == slot[i].Items.sprite)
                {
                    itemTooltipBackImg.gameObject.SetActive(true);
                    itemTooltipImg.sprite = slot[i].Items.sprite;
                    itemName.text = $"{slot[i].Items.ItemName}";
                    itemTooltipText.text = $"{slot[i].Items.ItemExplanation}";
                }
            }
            
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemTooltipBackImg.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)  // ����Ŭ���� �ش� ������ ��� (ETC�� �ƴҰ��)
    {
        float currentTimeClick = 0;  // Ŭ�� �̺�Ʈ
        currentTimeClick += Time.deltaTime;

        int ClickCount = eventData.clickCount;

        if (currentTimeClick <= 0.75f && ClickCount >= 2)  // 0.75�ʾȿ� �ѹ��������ٸ�
        {
            var result = eventData.pointerCurrentRaycast;
            Item.ItemData Tempitem = new Item.ItemData();

            #region ������ ��� ����
            if (result.gameObject.GetComponentInChildren<Slot>().Items != null)
            {
                for (int i = 0; i < 24; i++)
                {
                    if(result.gameObject.GetComponentInChildren<Slot>().Items == slot[i].Items && result.gameObject.GetComponentInChildren<Slot>().Items.sprite != null)
                    {
                        if (slot[i].Items.ItemTypes == Define.ItemType.Equipment) // ������Ÿ���� �����
                        {
                            #region ������� (��)
                            if (slot[i].Items.ItemName.Contains(" �� "))
                            {
                                if (Weapon_G.GetComponent<EquipSlot>().Items.sprite != null)  // ���â�� ��� �������Ͻ� ��� ���ν���    (�������۵��� ���� �ڵ尡 ������)
                                {
                                    Tempitem = Weapon_G.GetComponent<EquipSlot>().Items;
                                    Tempitem.Amount += 1;

                                    EquipSlot.equipList.Remove(Weapon_G.GetComponent<EquipSlot>().Items);

                                    Weapon_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.damage -= Tempitem.ItemUseStrength;
                                    GameManager.Player.PlayerController.Stat.damage += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    slot[i].Items = Tempitem;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }
                                if (Weapon_G.GetComponent<EquipSlot>().Items.sprite == null)  // ���â�� ��� �����Ǿ����� �ʴٸ� ��� ����    (�������۵��� ���� �ڵ尡 ������)
                                {
                                    Weapon_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.damage += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }
                            }
                            #endregion
                            #region ������� (����)
                            if (slot[i].Items.ItemName.Contains("����"))
                            {
                                if (Leggings_G.GetComponent<EquipSlot>().Items.sprite != null)
                                {
                                    Tempitem = Leggings_G.GetComponent<EquipSlot>().Items;
                                    Tempitem.Amount += 1;

                                    EquipSlot.equipList.Remove(Leggings_G.GetComponent<EquipSlot>().Items);

                                    Leggings_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp -= Tempitem.ItemUseStrength;
                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    slot[i].Items = Tempitem;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }
                                if (Leggings_G.GetComponent<EquipSlot>().Items.sprite == null)
                                {
                                    Leggings_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }
                            }
                            #endregion
                            #region ������� (����)
                            if (slot[i].Items.ItemName.Contains("����"))
                            {
                                if (Armor_G.GetComponent<EquipSlot>().Items.sprite != null)
                                {
                                    Tempitem = Armor_G.GetComponent<EquipSlot>().Items;
                                    Tempitem.Amount += 1;

                                    EquipSlot.equipList.Remove(Armor_G.GetComponent<EquipSlot>().Items);

                                    Armor_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp -= Tempitem.ItemUseStrength;
                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    slot[i].Items = Tempitem;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }
                                if (Armor_G.GetComponent<EquipSlot>().Items.sprite == null)
                                {
                                    Armor_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                } 
                            }
                            #endregion
                            #region ������� (�Ź�)
                            if (slot[i].Items.ItemName.Contains("�Ź�"))
                            {
                                if (Boots_G.GetComponent<EquipSlot>().Items.sprite != null)
                                {
                                    Tempitem = Boots_G.GetComponent<EquipSlot>().Items;
                                    Tempitem.Amount += 1;

                                    EquipSlot.equipList.Remove(Boots_G.GetComponent<EquipSlot>().Items);

                                    Boots_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp -= Tempitem.ItemUseStrength;
                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    slot[i].Items = Tempitem;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }
                                if (Boots_G.GetComponent<EquipSlot>().Items.sprite == null)
                                {
                                    Boots_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }
                            }
                            #endregion
                            #region ������� (����)
                            if (slot[i].Items.ItemName.Contains("����"))
                            {
                                if (Casque_G.GetComponent<EquipSlot>().Items.sprite != null)
                                {
                                    Tempitem = Casque_G.GetComponent<EquipSlot>().Items;
                                    Tempitem.Amount += 1;

                                    EquipSlot.equipList.Remove(Casque_G.GetComponent<EquipSlot>().Items);

                                    Casque_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp -= Tempitem.ItemUseStrength;
                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    slot[i].Items = Tempitem;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }
                                if (Casque_G.GetComponent<EquipSlot>().Items.sprite == null)
                                {
                                    Casque_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }  
                            }
                            #endregion
                            #region ������� (�尩)
                            if (slot[i].Items.ItemName.Contains("�尩"))
                            {
                                if (Gloves_G.GetComponent<EquipSlot>().Items.sprite != null)
                                {
                                    Tempitem = Gloves_G.GetComponent<EquipSlot>().Items;
                                    Tempitem.Amount += 1;

                                    EquipSlot.equipList.Remove(Gloves_G.GetComponent<EquipSlot>().Items);

                                    Gloves_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp -= Tempitem.ItemUseStrength;
                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    slot[i].Items = Tempitem;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }

                                if (Gloves_G.GetComponent<EquipSlot>().Items.sprite == null)
                                {
                                    Gloves_G.GetComponent<EquipSlot>().Items = slot[i].Items;

                                    EquipSlot.equipList.Add(slot[i].Items);

                                    GameManager.Player.PlayerController.Stat.maxHp += slot[i].Items.ItemUseStrength;
                                    slot[i].Items.Amount -= 1;

                                    GameManager.Sound.PlayEffectSound("Item/ChangeEquipSound");
                                }
                            }
                            #endregion

                        }
                        if (slot[i].Items.ItemTypes == Define.ItemType.Consumption) // ������Ÿ���� �Һ���
                        {
                            #region �Һ������ (��������) / Hp ȸ��
                            if (slot[i].Items.ItemName.Contains("��������"))
                            {
                                GameManager.Sound.PlayEffectSound("Item/PotionHeal");
                                GameManager.Player.PlayerController.Stat.Hp += slot[i].Items.ItemUseStrength;
                                slot[i].Items.Amount -= 1;
                            }
                            #endregion
                        }
                        if (slot[i].Items.Amount <= 0)  // ������ 0�̶�� Items������ Null�� ����
                        {
                            slot[i].Items = null;
                            itemTooltipBackImg.gameObject.SetActive(false);
                            return;
                        }
                    }
                    
                }
            }
            #endregion

        }
    }

    public static void ItemSave()
    {
        for (int i = 0; i < 24; i++)   
            InvenList.Add(slot[i]);
    }

    public static void ItemListClear()  // ����Ʈ�� ��÷���� �ʵ��� Ŭ����
    {
        InvenList.Clear();
    }
   
}

