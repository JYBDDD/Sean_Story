using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerSkill
{
    public string skillName;
    public int skillLev;
    public int skillDamage;
    public int coolTime;
    // 치명타확률은 BaseStat의 Critical_Chance에서 가져올것임

    public void SkillAttackDamage(BaseStat target, GameObject target1,BaseStat me)
    {
        float target1Random = UnityEngine.Random.Range(-0.5f, 0.5f);  // 타격이펙트 랜덤값 설정
        Vector3 target1RandomPositon = new Vector3(target1.transform.position.x + target1Random, target1.transform.position.y + 1.5f, target1.transform.position.z + target1Random);  // 타격이펙트 랜덤값

        if (BaseStat.CriticalRange > me.critical_Chance)  // 일반스킬
        {
            target.Hp -= BaseStat.DamageRange + skillDamage;
            GameManager.Sound.PlayEffectSound("HitSound");
            GameManager.Resource.Instantiate("UI/Particle_Effect/Player/UIEffect/PlayerAttackEffect", target1RandomPositon, Quaternion.identity, GameManager.staticHitEffectParent.transform);
            GameManager.Resource.Instantiate("UI/Particle_Effect/Player/PlayerDamageEffect", target1.transform.position + (Vector3.up * 3) + (Vector3.right), Quaternion.identity, GameManager.staticHitDamageParent.transform);
        }
        if(BaseStat.CriticalRange <= me.critical_Chance) // 치명타 스킬
        {
            target.Hp -= (BaseStat.DamageRange * 2) + (skillDamage  * 2);
            GameManager.Sound.PlayEffectSound("CriticalHitSound");
            GameManager.Resource.Instantiate("UI/Particle_Effect/Player/UIEffect/PlayerCriticalAttackEffect", target1RandomPositon, Quaternion.identity, GameManager.staticHitEffectParent.transform);
            GameManager.Resource.Instantiate("UI/Particle_Effect/Player/PlayerCriticalDamageEffect", target1.transform.position + (Vector3.up * 3) + (Vector3.right), Quaternion.identity, GameManager.staticHitDamageParent.transform);
        }
    }
}
