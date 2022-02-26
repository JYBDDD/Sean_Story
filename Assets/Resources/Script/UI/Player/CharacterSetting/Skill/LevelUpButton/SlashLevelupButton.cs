using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashLevelupButton : MonoBehaviour
{
    public void LevelUp()
    {
        if (CharacterBaseController.SkillPoint > 0 && SlashDataSet.staticSlashLev < 10) // ��ų����Ʈ�� �ְ�, ��ų������ 10�������� �۴ٸ�
        {
            CharacterBaseController.SkillPoint--;
            SlashDataSet.staticSlashLev++;
            GameManager.Sound.PlayEffectSound("Setting/SettingSound");
        }
    }
}
