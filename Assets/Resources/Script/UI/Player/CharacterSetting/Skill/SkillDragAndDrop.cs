using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]
    public Image BlankContainer; // 스킬을 임시로 저장하는 빈컨테이너
    protected Image dataImg;                 // 클릭한 스킬 데이터가 들어오도록 설정함          (해당 스킬 dataSet 에서 설정해주고 있음)
    protected PlayerSkill dataSkill;         // 클릭한 스킬 데이터를 넣어주도록 설정            (해당 스킬 dataSet 에서 설정해주고 있음)

    private void Start()
    {
        dataImg = null;
        dataSkill = null;

    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if(dataSkill.skillLev == 0 | dataSkill == null)  // skillLev이 0이라면 return 시킨다.
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
            if (BlankSkillButtonSet.skillList.Count > 2)   // 갯수가 3개가 될때 마지막 리스트 삭제
                BlankSkillButtonSet.skillList.RemoveAt(2);
        }
        
        

        BlankContainer.sprite = null;
        BlankContainer.gameObject.SetActive(false);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)  // 포인터가 오브젝트에 들어왔을때 툴팁 생성
    {

    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
