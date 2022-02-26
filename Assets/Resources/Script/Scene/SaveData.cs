using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // 캐릭터 위치 데이터
    public Vector3 playerVec;
    public Quaternion playerQut;

    // 캐릭터 스탯 데이터
    public int level;
    public int hp;
    public int maxHp;
    public int damage;
    public int critical_Chance;
    public int exp;
    public int maxExp;

    // 캐릭터 인벤토리 데이터
    public List<Item.ItemData> invenData = new List<Item.ItemData>();

    // 캐릭터 소비슬롯 데이터
    public List<Item.ItemData> consumData = new List<Item.ItemData>();

    // 캐릭터 스킬슬롯 데이터
    public List<PlayerSkill> skillData = new List<PlayerSkill>();

    // 장비 슬롯
    public List<Item.ItemData> equipmentData = new List<Item.ItemData>();
}
