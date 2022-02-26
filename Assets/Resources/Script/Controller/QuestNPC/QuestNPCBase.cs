using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Quest
{
    [System.Serializable]
    public class QuestData
    {
        public int questCode;             // 퀘스트 코드
        public string questNpcName;       // 퀘스트를 주는 NPC 이름
        public string questName;          // 퀘스트 이름
        public string questContent;       // 퀘스트 내용
        public string monsterName;        // 몬스터 이름
        public int completionConditions;  // 퀘스트 완료 조건
        public int compensation;          // 성공 보상
    }
}

public class QuestNPCBase : MonoBehaviour
{
    protected Quest.QuestData questDatas;

    public Quest.QuestData GetQuestData { get => questDatas; }

    public static Dictionary<int, Quest.QuestData> QuestDict = new Dictionary<int, Quest.QuestData>();

    private void Start()
    {
        #region 퀘스트 생성
        Quest.QuestData questData = new Quest.QuestData();
        questData.questCode = 10001;
        questData.questNpcName = "Guard_QuestNPC";
        questData.questName = "골렘 퇴치";
        questData.questContent = "골렘때문에 시끄러워서 잠을 잘수가 없군 \n 골렘 3마리를 처치해줘";
        questData.monsterName = "Golem";
        questData.completionConditions = 3;
        questData.compensation = 450;

        JsonConvert.SerializeObject(questData);
        GameManager.Json.ToJson(questData, "QuestJsonData");
        #endregion
    }
}
