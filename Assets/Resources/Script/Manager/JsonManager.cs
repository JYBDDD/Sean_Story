using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;

public class JsonManager
{
    public void ToJson<T>(T data, string savePath)
    {
        string jsonText = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.dataPath, $"Resources/Data/{savePath}.json");
        File.WriteAllText(path, jsonText);
    }

    public T FromJson<T>(string jsonDataName)
    {
        return JsonConvert.DeserializeObject<T>(GameManager.Resource.Load<TextAsset>($"Data/{jsonDataName}").text);
    }

    public void SaveData()  
    {
        SaveData data = new SaveData();

        data.playerVec = GameManager.Player.player.transform.position;  // ĳ���� ���� ������ ����
        data.playerQut = GameManager.Player.player.transform.rotation;
        data.level = GameManager.Player.PlayerController.Stat.Level;
        data.hp = GameManager.Player.PlayerController.Stat.Hp;
        data.maxHp = GameManager.Player.PlayerController.Stat.maxHp;
        data.exp = GameManager.Player.PlayerController.Stat.Exp;
        data.maxExp = GameManager.Player.PlayerController.Stat.maxExp;
        data.damage = GameManager.Player.PlayerController.Stat.damage;
        data.critical_Chance = GameManager.Player.PlayerController.Stat.critical_Chance;


        Slot[] slots = new Slot[24];  // �κ��丮 ������ ����
        for (int i = 0; i < 24; i++)
        {
            slots[i] = InventoryDragAndDrop.InvenList[i];
            if (slots[i] != null) 
                data.invenData.Add(slots[i].Items);

        }

        for (int i = 0; i < ConsumptionSlot.consumList.Count; i++)  // �Һ񽽷� ������ ����
            data.consumData.Add(ConsumptionSlot.consumList[i]);

        for (int i = 0; i < BlankSkillButtonSet.skillList.Count; i++)  // ��ų���� ������ ����
            data.skillData.Add(BlankSkillButtonSet.skillList[i]);

        for (int i = 0; i < EquipSlot.equipList.Count; i++)  // ��񽽷� ������ ����
            data.equipmentData.Add(EquipSlot.equipList[i]);


        string jsonText = JsonUtility.ToJson(data, true);
        string path = Application.persistentDataPath + "/SaveData.json";
        File.WriteAllText(path, jsonText);
    }

    public void LoadData(string jsonDataName)  // ĳ���� �׾����� �ε��
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData.json"))
        {
            string path = Application.persistentDataPath + "/SaveData.json";
            string jsonData = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);

            GameManager.Player.PlayerController.Stat.Level = saveData.level;
            GameManager.Player.PlayerController.Stat.Hp = saveData.maxHp;
            GameManager.Player.PlayerController.Stat.maxHp = saveData.maxHp;
            GameManager.Player.PlayerController.Stat.Exp = saveData.exp;
            GameManager.Player.PlayerController.Stat.maxExp = saveData.maxExp;
            GameManager.Player.PlayerController.Stat.damage = saveData.damage;
            GameManager.Player.PlayerController.Stat.critical_Chance = saveData.critical_Chance;
        }
    }

    public void PotalLoadData(string jsonDataName)
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData.json"))
        {
            string path = Application.persistentDataPath + "/SaveData.json";
            string jsonData = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);

            //GameManager.Player.player.transform.position = saveData.playerVec;   // ��Ż �̵��� �Ѵٸ� ���� ��Ż�� �������� �޾ƿ������ϱ�

            GameManager.Player.PlayerController.Stat.Level = saveData.level;
            GameManager.Player.PlayerController.Stat.Hp = saveData.hp;
            GameManager.Player.PlayerController.Stat.maxHp = saveData.maxHp;
            GameManager.Player.PlayerController.Stat.Exp = saveData.exp;
            GameManager.Player.PlayerController.Stat.maxExp = saveData.maxExp;
            GameManager.Player.PlayerController.Stat.damage = saveData.damage;
            GameManager.Player.PlayerController.Stat.critical_Chance = saveData.critical_Chance;


        }

    }

    public void InvenLoadData(string jsonDataName)
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData.json"))
        {
            string path = Application.persistentDataPath + "/SaveData.json";
            string jsonData = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);

            for (int i = 0; i < 24; i++)
                InventoryDragAndDrop.slot[i].Items = saveData.invenData[i];
        }
 

    }

    public void ConsumLoadData(string jsonDataName)
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData.json"))
        {
            string path = Application.persistentDataPath + "/SaveData.json";
            string jsonData = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);

            for (int i = 0; i < saveData.consumData.Count; i++)
                BaseConsumSlot.consumSlot[i].Items = saveData.consumData[i];
        }


    }

    public void SkillLoadData(string jsonDataName)
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData.json"))
        {
            string path = Application.persistentDataPath + "/SaveData.json";
            string jsonData = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);

            for (int i = 0; i < saveData.skillData.Count; i++)
            {
                BaseBlankSkillButton.skillSlot[i].Skill = saveData.skillData[i];
                if (BaseBlankSkillButton.skillSlot[i].Skill.skillName == "Lightning Enchantment")
                    BaseBlankSkillButton.skillSlot[i].thisImg.sprite = EnchantDataSet.thisImg.sprite;
                if (BaseBlankSkillButton.skillSlot[i].Skill.skillName == "Slash")
                    BaseBlankSkillButton.skillSlot[i].thisImg.sprite = SlashDataSet.thisImg.sprite;
            }
        }


    }
    public void EquipLoadData(string jsonDataName)
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData.json"))
        {
            string path = Application.persistentDataPath + "/SaveData.json";
            string jsonData = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);

            for (int i = 0; i < saveData.equipmentData.Count; i++)
            {
                if (saveData.equipmentData[i].ItemName.Contains("����"))
                    BaseEquipSlot.equipSlot[0].Items = saveData.equipmentData[i];
                if (saveData.equipmentData[i].ItemName.Contains("����"))
                    BaseEquipSlot.equipSlot[1].Items = saveData.equipmentData[i];
                if (saveData.equipmentData[i].ItemName.Contains("����"))
                    BaseEquipSlot.equipSlot[2].Items = saveData.equipmentData[i];
                if (saveData.equipmentData[i].ItemName.Contains(" �� "))
                    BaseEquipSlot.equipSlot[3].Items = saveData.equipmentData[i];
                if (saveData.equipmentData[i].ItemName.Contains("�尩"))
                    BaseEquipSlot.equipSlot[4].Items = saveData.equipmentData[i];
                if (saveData.equipmentData[i].ItemName.Contains("�Ź�"))
                    BaseEquipSlot.equipSlot[5].Items = saveData.equipmentData[i];
            }
        }

            
        
    }
}
