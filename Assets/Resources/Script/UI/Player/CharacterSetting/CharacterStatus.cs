using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatus : MonoBehaviour
{
    [SerializeField]
    private Text level;
    [SerializeField]
    private Text power;
    [SerializeField]
    private Text hp;
    [SerializeField]
    private Text critical;
    [SerializeField]
    private Text exp;
    [Space]

    // �������� ������ �ɷ�ġ ��� ǥ�ÿ�
    [SerializeField]
    private EquipSlot Casque;
    [SerializeField]
    private EquipSlot Armor;
    [SerializeField]
    private EquipSlot Leggings;
    [SerializeField]
    private EquipSlot Weapon;
    [SerializeField]
    private EquipSlot Gloves;
    [SerializeField]
    private EquipSlot Boots;

    private void Update() // ����â�� �����ִٸ� Update ��Ų��
    {
        UpdateStat();
    }

    private void UpdateStat()
    {
        level.text = $" Level : { GameManager.Player.PlayerController.Stat.level}";
        power.text = $" Damage\t:\t { GameManager.Player.PlayerController.Stat.damage}\t\t ��({ Weapon.Items.ItemUseStrength})";
        hp.text = $" MaxHp\t:\t { GameManager.Player.PlayerController.Stat.maxHp}\t ��({Casque.Items.ItemUseStrength + Armor.Items.ItemUseStrength + Leggings.Items.ItemUseStrength + Gloves.Items.ItemUseStrength + Boots.Items.ItemUseStrength})";
        critical.text = $" Critical\t:\t { GameManager.Player.PlayerController.Stat.critical_Chance}\t\t";
        exp.text = $" Exp\t\t\t:\t { GameManager.Player.PlayerController.Stat.exp}\t\t";
    }
}
