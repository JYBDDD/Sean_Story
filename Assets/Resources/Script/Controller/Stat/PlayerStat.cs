using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStat : BaseStat
{
    public int exp;
    public int maxExp;

    public override int Level
    {
        get => base.level;
        set
        {
            base.Level = value;
            GameManager.Player.PlayerController.Stat = PlayerAllData.StatDict[base.Level] as PlayerStat;
            // �ش� ĳ���� = �ش� ĳ���� ����
        }
    }

    public int Exp
    {
        get => exp;
        set
        {
            exp = value;
            if (exp >= maxExp)
            {
                Level++;
                exp -= maxExp;
                CharacterBaseController.SkillPoint++;  // ��ų����Ʈ +
                GameManager.Resource.Instantiate("UI/Particle_Effect/Player/UIEffect/PlayerLevelUp", GameManager.Player.player.transform.position + (Vector3.up * 4), Quaternion.identity, GameManager.staticPlayerParent.transform);
            }
        }
    }
}
