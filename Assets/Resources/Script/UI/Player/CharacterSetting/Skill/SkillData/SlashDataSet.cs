using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlashDataSet : SkillDragAndDrop
{
    [SerializeField]
    private Text skill_slashLev;
    PlayerSkill skill;
    public static Image thisImg;

    [SerializeField]
    private Image skillTooltipImg;
    [SerializeField]
    private Text skillTooltipText;
    [SerializeField]
    private Image tooltipImg;

    public static int staticSlashLev = 0;  // static ������ ����

    private void Start()
    {
        thisImg = GetComponent<Image>();
        skillTooltipImg.enabled = false;
        skillTooltipText.enabled = false;
        tooltipImg.enabled = false;
    }

    private void Update()
    {
        EnchantSetting();
    }

    private void EnchantSetting()
    {
        skill = PlayerAllData.SkillSlashDict[staticSlashLev];
        skill_slashLev.text = $"Lev : {PlayerAllData.SkillSlashDict[staticSlashLev].skillLev}";
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        dataImg = thisImg;
        dataSkill = skill;
        base.OnBeginDrag(eventData);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        dataImg = thisImg;
        dataSkill = skill;

        if (dataSkill.skillLev == 0 | dataSkill == null)  // skillLev�� 0�̶�� return ��Ų��.
        {
            return;
        }

        base.OnPointerEnter(eventData);
        skillTooltipImg.enabled = true;
        skillTooltipText.enabled = true;
        tooltipImg.enabled = true;
        tooltipImg.sprite = dataImg.sprite;

        skillTooltipText.text = 
            $"��ų�� : {dataSkill.skillName}\n\n��Ÿ�� : {dataSkill.coolTime}\n\n������ : (���ݷ�100% + {dataSkill.skillDamage})\n * ��ųŸ��Ƚ��";
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        skillTooltipImg.enabled = false;
        skillTooltipText.enabled = false;
        tooltipImg.enabled = false;
    }

}
