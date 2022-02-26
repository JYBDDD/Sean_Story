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
            if (QuestNPCController.QuestNPCName == QuestNPCBase.QuestDict[i].questNpcName)  // ���� ����Ʈ�� �ִ� NPC �̸��� ����Ʈ��ųʸ��� �� NPC�̸��� ���ٸ�
            {
                questText.text = $"{QuestNPCBase.QuestDict[i].questName}\n\n{QuestNPCBase.QuestDict[i].questContent}\n\n�Ϸ����� : {QuestNPCBase.QuestDict[i].completionConditions}���� óġ\n�������� : {QuestNPCBase.QuestDict[i].compensation}�� ����ġ";

            }
        }
        
    }
}
