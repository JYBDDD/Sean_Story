using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlankSkillButtonSet : SkillDragAndDrop
{
    public PlayerSkill skill;

    public PlayerSkill Skill
    {
        get { return skill; }
        set { skill = value; }
    }

    public Image thisImg;

    public static List<PlayerSkill> skillList = new List<PlayerSkill>();

    private void Awake()
    {
        thisImg = GetComponent<Image>();
        skill = null;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (Skill == null | Skill != null)  // ��ųâ���� ���� �����ܿ��°��� �ƴ϶�� ������ �Ұ����ϰ� ���Ƴ���
            return;
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (Skill == null | Skill != null)
            return;
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (Skill == null | Skill != null)
            return;
        base.OnEndDrag(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void PushSkillButton()  // ��ų ��ư �̺�Ʈ
    {
        if (skill == null)
            return;
        if(skill.skillName == "Lightning Enchantment")
        {
            CharacterBaseController.IsEnChantUse = true;
        }
        if(skill.skillName == "Slash")
        {
            CharacterBaseController.IsSlashUse = true;
        }
    }
    


}
