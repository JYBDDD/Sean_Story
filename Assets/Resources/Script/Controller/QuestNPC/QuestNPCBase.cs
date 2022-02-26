using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Quest
{
    [System.Serializable]
    public class QuestData
    {
        public int questCode;             // ����Ʈ �ڵ�
        public string questNpcName;       // ����Ʈ�� �ִ� NPC �̸�
        public string questName;          // ����Ʈ �̸�
        public string questContent;       // ����Ʈ ����
        public string monsterName;        // ���� �̸�
        public int completionConditions;  // ����Ʈ �Ϸ� ����
        public int compensation;          // ���� ����
    }
}

public class QuestNPCBase : MonoBehaviour
{
    protected Quest.QuestData questDatas;

    public Quest.QuestData GetQuestData { get => questDatas; }

    public static Dictionary<int, Quest.QuestData> QuestDict = new Dictionary<int, Quest.QuestData>();

    private void Start()
    {
        #region ����Ʈ ����
        Quest.QuestData questData = new Quest.QuestData();
        questData.questCode = 10001;
        questData.questNpcName = "Guard_QuestNPC";
        questData.questName = "�� ��ġ";
        questData.questContent = "�񷽶����� �ò������� ���� �߼��� ���� \n �� 3������ óġ����";
        questData.monsterName = "Golem";
        questData.completionConditions = 3;
        questData.compensation = 450;

        JsonConvert.SerializeObject(questData);
        GameManager.Json.ToJson(questData, "QuestJsonData");
        #endregion
    }
}
