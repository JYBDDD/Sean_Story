using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-3)]
public class PlayerSpawner : MonoBehaviour
{
    private void Awake()
    {
        if (GameManager.Player.player != null)
            return;

        GameManager.Player.player = GameManager.Resource.Instantiate("Character/Player", transform.position, Quaternion.identity);
        GameManager.Player.PlayerController = GameManager.Player.player.GetComponent<CharacterBaseController>();

        if (PotalSceneLoad.staticPotaling == true)  // ��Ż�� ���ٸ� ��Ż��ġ�� �̵������ش�
        {
            InventoryDragAndDrop.InvenLoadBool = true;
            BaseConsumSlot.ConsumBool = true;
            BaseBlankSkillButton.SkillBool = true;
            BaseEquipSlot.EquipBool = true;
            GameManager.Json.PotalLoadData("SaveData");
            GameManager.Player.player.transform.position = GameObject.FindObjectOfType<PotalSceneLoad>().transform.position + (Vector3.back * 2);
        }


        if (PotalSceneLoad.StaticMoveSceneName == "SeanIsland")
            GameManager.Sound.PlayBGM("SeanIsland_BGM");
        if (PotalSceneLoad.StaticMoveSceneName == "DarkRoad")
            GameManager.Sound.PlayBGM("DarkRoad_BGM");
    }
    private void Start()
    {
        PotalSceneLoad.staticPotaling = false;  // ĳ���� ������ ��Ż�� false
    }

    private void Update()
    {
        if(PlayerDeadEffectController.IsDead == true)  // ĳ���Ͱ� �׾����� ����� (�ش�ʿ��� �����)
        {
            GameManager.Json.LoadData("SaveData");
            ReSetCharacter();
            PlayerDeadEffectController.IsDead = false;
        }
    }

    private void ReSetCharacter()
    {
        GameManager.Player.player = GameManager.Resource.CharacterRecycle("Character/Player", transform.position, Quaternion.identity);
        GameManager.Player.PlayerController = GameManager.Player.player.GetComponent<CharacterBaseController>();
        PlayerJoyStick.characterBaseController = GameManager.Player.PlayerController;
    }

}
