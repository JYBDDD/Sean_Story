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
            if (QuestNPCController.QuestNPCName == QuestNPCBase.QuestDict[i].questNpcName)  // ���� ����Ʈ�� �ִ� NPC �̸��� ����Ʈ��ųʸ��� �� NPC�̸��� ���ٸ�
            {
                questText.text = $"{QuestNPCBase.QuestDict[i].questName}\n\n {QuestNPCController.QuestProgress}/{QuestNPCBase.QuestDict[i].completionConditions}���� óġ";

            }
        }
    }
}
