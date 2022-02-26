using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPCController : QuestNPCBase
{
    [SerializeField]
    private GameObject conversationButton;
    [SerializeField]
    private GameObject questScessprogress;

    private bool OneCheck = false;
    public static bool buttonCheck = false;
    private GameObject QuestMark;

    public static string QuestNPCName;
    public static int QuestProgress = 0;

    private void Start()
    {
        OneCheck = false;
        buttonCheck = false;
        questDatas = JsonConvert.DeserializeObject<Quest.QuestData>(GameManager.Resource.Load<TextAsset>("Data/Quest/QuestJsonData").text);
        QuestMark = GameManager.Resource.Instantiate("UI/QuestMark", transform.position + (Vector3.up * 2) - (Vector3.right * 1.5f), Quaternion.Euler(0, 180, 0), transform);
        QuestMarkColor(0);
    }

    private void Update()
    {
        if (QuestButton.Yesbutton == true && buttonCheck == false)
        {
            QuestMarkColor(0);
            buttonCheck = true;
        }

        if(GameManager.Player.PlayerController.Stat.Level >= 2 && OneCheck == false)
        {
            QuestDict.Add(questDatas.questCode,questDatas);
            OneCheck = true;
            QuestMarkColor(1);
        }

        if(QuestDict.Count >= 1)
        {
            if (QuestProgress >= QuestDict[questDatas.questCode].completionConditions)
            {
                QuestMarkColor(1);
            }
        }

    }

    private void QuestMarkColor(float alpha)  // 퀘스트 마크 컬러값
    {
        Color color = QuestMark.GetComponent<SpriteRenderer>().color;
        color.a = alpha;
        QuestMark.GetComponent<SpriteRenderer>().color = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (QuestDict.Count != 0)
        {
            if (QuestProgress >= QuestDict[questDatas.questCode].completionConditions)
            {
                questScessprogress.SetActive(true);
                if(QuestScessSet.QuestScessBool == true)
                {
                    GameManager.Player.PlayerController.Stat.Exp += QuestDict[questDatas.questCode].compensation;
                    QuestDict.Remove(questDatas.questCode);
                    QuestProgress = 0;
                    QuestScessSet.QuestScessBool = false;
                    QuestMarkColor(0);
                }
            }
        }
            
        if(QuestDict.Count > 0 && QuestProgress >= 0)  // 퀘스트가 있다면
        {
            conversationButton.SetActive(true);
            QuestNPCName = gameObject.name;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (conversationButton == null)
            return;
        if(conversationButton.activeSelf)
            conversationButton.SetActive(false);
    }
}
