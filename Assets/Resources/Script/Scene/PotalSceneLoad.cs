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
            InventoryDragAndDrop.ItemSave();       // 아이템Save를 하기전에  인벤토리가 면저 켜지도록 해야함
            GameManager.Json.SaveData();
            SceneManager.LoadScene("LoadingScene");
            StaticMoveSceneName = MoveSceneName;
            GameManager.Sound.PlayEffectSound("Potal/PotalSound");
        }

    }
}
