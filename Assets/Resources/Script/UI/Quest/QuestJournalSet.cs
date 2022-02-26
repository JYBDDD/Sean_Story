using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestJournalSet : MonoBehaviour
{
    private Text questText;

    private void OnEnable()
    {
        questText = GetComponentInChildren<Text>();
        TextSet();
    }

    private void Update()
    {
        if(QuestButton.Yesbutton == true)
        {
            gameObject.SetActive(false);
            QuestButton.Yesbutton = false;
        }

        if(QuestButton.Nobutton == true)
        {
            gameObject.SetActive(false);
            QuestButton.Nobutton = false;
        }
    }

    private void TextSet()
    {
        for (int i = 10001; i < QuestNPCBase.QuestDict.Count + 10001; i++)
        {
            if (QuestNPCController.QuestNPCName == QuestNPCBase.QuestDict[i].questNpcName)  // 현재 퀘스트를 주는 NPC 이름과 퀘스트딕셔너리에 들어간 NPC이름이 같다면
            {
                questText.text = $"{QuestNPCBase.QuestDict[i].questName}\n\n{QuestNPCBase.QuestDict[i].questContent}\n\n완료조건 : {QuestNPCBase.QuestDict[i].completionConditions}마리 처치\n성공보상 : {QuestNPCBase.QuestDict[i].compensation}의 경험치";

            }
        }
        
    }
}
