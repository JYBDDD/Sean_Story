using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class PlayerStatData
{
    public PlayerStat[] playerStat = new PlayerStat[50];
}

[System.Serializable]
public class PlayerSkillData
{
    public PlayerSkill[] playerSkill = new PlayerSkill[11];
}

public class PlayerAllData : IManager
{
    public static Dictionary<int, BaseStat> StatDict = new Dictionary<int, BaseStat>(); // �÷��̾� ����

    public static Dictionary<int, PlayerSkill> SkillEnchantDict = new Dictionary<int, PlayerSkill>(); // ��þƮ ��ų
    public static Dictionary<int, PlayerSkill> SkillSlashDict = new Dictionary<int, PlayerSkill>();  // ������ ��ų

    public void Init()
    {
        #region ���Ⱥҷ�����
        PlayerStatData data = GameManager.Json.FromJson<PlayerStatData>("Player/PlayerStatData");

        for (int i = 0; i < data.playerStat.Length; i++)
        {
            StatDict.Add(data.playerStat[i].level, data.playerStat[i]);
        }
        #endregion

        #region ��ų�ҷ�����
        PlayerSkillData slashData = GameManager.Json.FromJson<PlayerSkillData>("Player/Skill/PlayerSkill_Slash");
        PlayerSkillData lightningEnchantData = GameManager.Json.FromJson<PlayerSkillData>("Player/Skill/PlayerSkill_LightningEnchantment");

        for (int i = 0; i < slashData.playerSkill.Length; i++)
        {
            SkillSlashDict.Add(slashData.playerSkill[i].skillLev, slashData.playerSkill[i]);
        }
        for (int i = 0; i < lightningEnchantData.playerSkill.Length; i++)
        {
            SkillEnchantDict.Add(lightningEnchantData.playerSkill[i].skillLev, lightningEnchantData.playerSkill[i]);
        }
        #endregion  // �ϴ� ��ų�� ������� ���� (��ųâ���� ȣ������ָ� �ɵ�) = ������ ������  ��ų����Ʈ�� ��� ����Ʈ�� �����ϸ� ��ų������ ����
    }

    public void OnUpdate()
    {
        
    }

    public void Clear()
    {

    }

    

}
