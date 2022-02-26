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
    public static Dictionary<int, BaseStat> StatDict = new Dictionary<int, BaseStat>(); // 플레이어 스탯

    public static Dictionary<int, PlayerSkill> SkillEnchantDict = new Dictionary<int, PlayerSkill>(); // 인첸트 스킬
    public static Dictionary<int, PlayerSkill> SkillSlashDict = new Dictionary<int, PlayerSkill>();  // 슬래쉬 스킬

    public void Init()
    {
        #region 스탯불러오기
        PlayerStatData data = GameManager.Json.FromJson<PlayerStatData>("Player/PlayerStatData");

        for (int i = 0; i < data.playerStat.Length; i++)
        {
            StatDict.Add(data.playerStat[i].level, data.playerStat[i]);
        }
        #endregion

        #region 스킬불러오기
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
        #endregion  // 일단 스킬을 저장시켜 놓음 (스킬창에서 호출시켜주면 될듯) = 레벨이 오를시  스킬포인트를 얻고 포인트를 투자하면 스킬레벨이 오름
    }

    public void OnUpdate()
    {
        
    }

    public void Clear()
    {

    }

    

}
