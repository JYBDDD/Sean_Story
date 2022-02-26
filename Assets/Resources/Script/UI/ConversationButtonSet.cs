using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationButtonSet : MonoBehaviour
{
    [SerializeField]
    private GameObject QuestJournal;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnClickEnterQuest()
    {
        QuestJournal.SetActive(true);
    }
}
