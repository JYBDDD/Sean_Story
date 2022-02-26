using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantLevelupButton : MonoBehaviour
{

    public void LevelUp()
    {
        if(CharacterBaseController.SkillPoint > 0 && EnchantDataSet.staticEnchantLev < 10) // 스킬포인트가 있고, 스킬레벨이 10레벨보다 작다면
        {
            CharacterBaseController.SkillPoint--;
            EnchantDataSet.staticEnchantLev++;
            GameManager.Sound.PlayEffectSound("Setting/SettingSound");
        }
    }
}
