using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;

    public static GameManager Instance { get { Init(); return _instance; } }

    PlayerManager _player = new PlayerManager();
    ResourceManager _resource = new ResourceManager();
    JsonManager _json = new JsonManager();
    PlayerAllData _playAllData = new PlayerAllData();
    PoolManager _pool = new PoolManager();
    InventoryManager _inven = new InventoryManager();
    SoundManager _sound = new SoundManager();
    MonsterBaseController _monsterBase;
    InventoryDragAndDrop _invenD;


    public static PlayerManager Player { get => Instance._player; }
    public static ResourceManager Resource { get => Instance._resource; }
    public static JsonManager Json { get => Instance._json; }
    public static PlayerAllData PlayerAllData { get => Instance._playAllData; }
    public static PoolManager Pool { get => Instance._pool; }
    public static InventoryManager Inven { get => Instance._inven; }
    public static SoundManager Sound { get => Instance._sound; }
    public static MonsterBaseController MonsterBase { get => Instance._monsterBase; }
    public static InventoryDragAndDrop InvenD { get => Instance._invenD; }


    public static GameObject staticParent;
    public static GameObject staticPlayerParent;
    public static GameObject staticHitEffectParent;
    public static GameObject staticHitDamageParent;
    public static GameObject staticItemParent;
    public static GameObject staticSoundParent;

    static void Init()
    {
        if(_instance == null)
        {
            GameObject go = GameObject.Find("@GameManager");
            if (go == null)
            {
                go = new GameObject { name = "@GameManager" };
                go.AddComponent<GameManager>();
            }
            _instance = go.GetComponent<GameManager>();
            DontDestroyOnLoad(go);

            staticParent = new GameObject { name = "@Root_MonsterParent"};   // 모든 몬스터 오브젝트의 부모로 설정
            DontDestroyOnLoad(staticParent);
            staticPlayerParent = new GameObject { name = "@Root_PlayerParent" };  // 모든 플레이어 오브젝트의 부모로 설정
            DontDestroyOnLoad(staticPlayerParent);
            staticHitEffectParent = new GameObject { name = "@Root_HitEffectParent" }; // 타격 이펙트 오브젝트의 부모로 설정
            DontDestroyOnLoad(staticHitEffectParent);
            staticHitDamageParent = new GameObject { name = "@Root_HitDamageParent" };  // 타격 데미지 오브젝트의 부모로 설정
            DontDestroyOnLoad(staticHitDamageParent);
            staticItemParent = new GameObject { name = "@Root_ItemParent" }; // 아이템 오브젝트의 부모로 설정
            DontDestroyOnLoad(staticItemParent);
            staticSoundParent = new GameObject { name = "@Root_SoundParent" }; // 사운드 오브젝트의 부모로 설정
            DontDestroyOnLoad(staticSoundParent);

            _instance._playAllData.Init();
            _instance._resource.Init();
        }

    }
    private void Update()
    {
        _instance._playAllData.OnUpdate();
    }

    public static void Clear()   // 씬옮길시 자식오브젝트 초기화 중 (LoadingSceneBar 에서 사용중) ※ 오류방지
    {
        Pool.Clear();

        var child = staticParent.transform.gameObject;
        for(int i=0;i<staticParent.transform.childCount;i++)
        {
            child = staticParent.transform.GetChild(i).gameObject;
            Destroy(child);
        }

        child = staticPlayerParent.transform.gameObject;
        for(int i =0;i<staticPlayerParent.transform.childCount;i++)
        {
            child = staticPlayerParent.transform.GetChild(i).gameObject;
            Destroy(child);
        }

        child = staticHitEffectParent.transform.gameObject;
        for (int i = 0; i < staticHitEffectParent.transform.childCount; i++)
        {
            child = staticHitEffectParent.transform.GetChild(i).gameObject;
            Destroy(child);
        }

        child = staticHitDamageParent.transform.gameObject;
        for (int i = 0; i < staticHitDamageParent.transform.childCount; i++)
        {
            child = staticHitDamageParent.transform.GetChild(i).gameObject;
            Destroy(child);
        }
        child = staticItemParent.transform.gameObject;
        for (int i = 0; i < staticItemParent.transform.childCount; i++)
        {
            child = staticItemParent.transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }
}
