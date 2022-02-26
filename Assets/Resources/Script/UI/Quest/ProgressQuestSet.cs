using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressQuestSet : MonoBehaviour
{
    private Text questText;

    private void Start()
    {
        questText = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if (QuestNPCBase.QuestDict.Count <= 0)
        {
            gameObject.SetActive(false);
            return;
        }
        ProgressText();
    }

    private void ProgressText()
    {
        for (int i = 10001; i < QuestNPCBase.QuestDict.Count + 10001; i++)
        {
            if (QuestNPCController.QuestNPCName == QuestNPCBase.QuestDict[i].questNpcName)  // 현재 퀘스트를 주는 NPC 이름과 퀘스트딕셔너리에 들어간 NPC이름이 같다면
            {
                questText.text = $"{QuestNPCBase.QuestDict[i].questName}\n\n {QuestNPCController.QuestProgress}/{QuestNPCBase.QuestDict[i].completionConditions}마리 처치";

            }
        }
    }
}
