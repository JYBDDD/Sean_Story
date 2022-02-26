using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]
    public Image BlankContainer; // ��ų�� �ӽ÷� �����ϴ� �������̳�
    protected Image dataImg;                 // Ŭ���� ��ų �����Ͱ� �������� ������          (�ش� ��ų dataSet ���� �������ְ� ����)
    protected PlayerSkill dataSkill;         // Ŭ���� ��ų �����͸� �־��ֵ��� ����            (�ش� ��ų dataSet ���� �������ְ� ����)

    private void Start()
    {
        dataImg = null;
        dataSkill = null;

    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if(dataSkill.skillLev == 0 | dataSkill == null)  // skillLev�� 0�̶�� return ��Ų��.
        {
            BlankContainer.gameObject.SetActive(false);
            return;
        }
        BlankContainer.gameObject.SetActive(true);
        BlankContainer.sprite = dataImg.sprite;  
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        BlankContainer.transform.position = eventData.position;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if(dataSkill.skillLev == 0 | dataSkill == null)
        {
            return;
        }

        var result = eventData.pointerCurrentRaycast;

        if(result.gameObject.GetComponent<BlankSkillButtonSet>() != null)
        {
            var targetImage = result.gameObject.GetComponent<Image>();
            var targetSkill = result.gameObject.GetComponent<BlankSkillButtonSet>();

            if(targetSkill.Skill != null)
            {
                if (targetSkill.Skill != dataSkill && targetImage != BlankContainer.sprite)
                {
                    if (BlankSkillButtonSet.skillList[0].skillName != dataSkill.skillName)
                        BlankSkillButtonSet.skillList.RemoveAt(0);

                    if (BlankSkillButtonSet.skillList.Count > 1)
                        if (BlankSkillButtonSet.skillList[1].skillName != dataSkill.skillName)
                            BlankSkillButtonSet.skillList.RemoveAt(1);
                }
            }
            

            targetImage.sprite = BlankContainer.sprite;
            targetSkill.skill = dataSkill;
            BlankSkillButtonSet.skillList.Add(dataSkill);
            if (BlankSkillButtonSet.skillList.Count > 2)   // ������ 3���� �ɶ� ������ ����Ʈ ����
                BlankSkillButtonSet.skillList.RemoveAt(2);
        }
        
        

        BlankContainer.sprite = null;
        BlankContainer.gameObject.SetActive(false);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)  // �����Ͱ� ������Ʈ�� �������� ���� ����
    {

    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
