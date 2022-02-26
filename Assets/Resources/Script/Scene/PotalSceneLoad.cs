using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PotalSceneLoad : MonoBehaviour
{
    public static string StaticMoveSceneName = "SeanIsland";
    public static bool staticPotaling = false;

    [SerializeField]
    public string MoveSceneName;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            staticPotaling = true;
            InventoryDragAndDrop.ItemSave();       // ������Save�� �ϱ�����  �κ��丮�� ���� �������� �ؾ���
            GameManager.Json.SaveData();
            SceneManager.LoadScene("LoadingScene");
            StaticMoveSceneName = MoveSceneName;
            GameManager.Sound.PlayEffectSound("Potal/PotalSound");
        }

    }
}
