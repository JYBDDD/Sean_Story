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
    // ġ��ŸȮ���� BaseStat�� Critical_Chance���� �����ð���

    public void SkillAttackDamage(BaseStat target, GameObject target1,BaseStat me)
    {
        float target1Random = UnityEngine.Random.Range(-0.5f, 0.5f);  // Ÿ������Ʈ ������ ����
        Vector3 target1RandomPositon = new Vector3(target1.transform.position.x + target1Random, target1.transform.position.y + 1.5f, target1.transform.position.z + target1Random);  // Ÿ������Ʈ ������

        if (BaseStat.CriticalRange > me.critical_Chance)  // �Ϲݽ�ų
        {
            target.Hp -= BaseStat.DamageRange + skillDamage;
            GameManager.Sound.PlayEffectSound("HitSound");
            GameManager.Resource.Instantiate("UI/Particle_Effect/Player/UIEffect/PlayerAttackEffect", target1RandomPositon, Quaternion.identity, GameManager.staticHitEffectParent.transform);
            GameManager.Resource.Instantiate("UI/Particle_Effect/Player/PlayerDamageEffect", target1.transform.position + (Vector3.up * 3) + (Vector3.right), Quaternion.identity, GameManager.staticHitDamageParent.transform);
        }
        if(BaseStat.CriticalRange <= me.critical_Chance) // ġ��Ÿ ��ų
        {
            target.Hp -= (BaseStat.DamageRange * 2) + (skillDamage  * 2);
            GameManager.Sound.PlayEffectSound("CriticalHitSound");
            GameManager.Resource.Instantiate("UI/Particle_Effect/Player/UIEffect/PlayerCriticalAttackEffect", target1RandomPositon, Quaternion.identity, GameManager.staticHitEffectParent.transform);
            GameManager.Resource.Instantiate("UI/Particle_Effect/Player/PlayerCriticalDamageEffect", target1.transform.position + (Vector3.up * 3) + (Vector3.right), Quaternion.identity, GameManager.staticHitDamageParent.transform);
        }
    }
}
