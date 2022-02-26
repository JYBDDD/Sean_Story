using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // ĳ���� ��ġ ������
    public Vector3 playerVec;
    public Quaternion playerQut;

    // ĳ���� ���� ������
    public int level;
    public int hp;
    public int maxHp;
    public int damage;
    public int critical_Chance;
    public int exp;
    public int maxExp;

    // ĳ���� �κ��丮 ������
    public List<Item.ItemData> invenData = new List<Item.ItemData>();

    // ĳ���� �Һ񽽷� ������
    public List<Item.ItemData> consumData = new List<Item.ItemData>();

    // ĳ���� ��ų���� ������
    public List<PlayerSkill> skillData = new List<PlayerSkill>();

    // ��� ����
    public List<Item.ItemData> equipmentData = new List<Item.ItemData>();
}
