using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantLevelupButton : MonoBehaviour
{

    public void LevelUp()
    {
        if(CharacterBaseController.SkillPoint > 0 && EnchantDataSet.staticEnchantLev < 10) // ��ų����Ʈ�� �ְ�, ��ų������ 10�������� �۴ٸ�
        {
            CharacterBaseController.SkillPoint--;
            EnchantDataSet.staticEnchantLev++;
            GameManager.Sound.PlayEffectSound("Setting/SettingSound");
        }
    }
}
