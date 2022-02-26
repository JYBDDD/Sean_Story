using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : IManager
{
    public Dictionary<string, AudioClip> AudioClipDict = new Dictionary<string, AudioClip>();

    public GameObject Bgm_Obj = null;  // BGM 오브젝트
    public AudioSource Bgm_Src = null; // BGM 오디오소스

    public void Load_AudioClipList()
    {
        AudioClip a_AudioClip = null;

        object[] temp = Resources.LoadAll("Art/Sound");

        for(int i =0; i< temp.Length;i++)
        {
            a_AudioClip = temp[i] as AudioClip;
            AudioClipDict.Add(a_AudioClip.name, a_AudioClip);
        }
    }

    public void Load_BGM()
    {
        if(Bgm_Obj == null)
        {
            Bgm_Obj = new GameObject();
            Bgm_Obj.name = "BgmSoundObj";

            Bgm_Obj.transform.SetParent(GameManager.staticSoundParent.transform);
            Bgm_Obj.transform.position = Vector3.zero;

            Bgm_Src = Bgm_Obj.AddComponent<AudioSource>();
            Bgm_Src.playOnAwake = false;
        }

    }

    public void PlayBGM(string a_FileName) // 재생
    {
        AudioClip a_AudioClip = null;

        if (AudioClipDict.ContainsKey(a_FileName) == true)
            a_AudioClip = AudioClipDict[a_FileName] as AudioClip;

        else
        {
            a_AudioClip = Resources.Load("Art/Sound/BGM/" + a_FileName) as AudioClip;
            AudioClipDict.Add(a_FileName, a_AudioClip);
        }

        Load_BGM();


        if(a_AudioClip != null & Bgm_Src != null)
        {
            Bgm_Src.clip = a_AudioClip;
            Bgm_Src.volume = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
            Bgm_Src.loop = true;
            Bgm_Src.Play(0);
        }
    }

    public float m_BgmVolume = 0;
    public float m_EffectVolume = 0;

    public ArrayList m_Effect_Obj_List = new ArrayList();   // 사운드 오브젝트 리스트(Vector)
    public AudioSource[] m_Effect_Src_List = new AudioSource[10]; // 사운드 오브젝트 오디오 소스 리스트

    private int Effect_Max_Count = 10;  // 최대 10개
    public int m_Effect_Index = 0;     // 현재 재생 중인 AudioSource 인덱스

    public void SetBgmVolume(float fVolume)
    {
        m_BgmVolume = fVolume;
        if (Bgm_Src != null)
            Bgm_Src.volume = m_BgmVolume;
    }

    public void SetEffectVolume(float fVolume)
    {
        m_EffectVolume = fVolume;

        for (int i = 0; i < m_Effect_Src_List.Length; i++)
        {
            if (m_Effect_Src_List[i] != null)
                m_Effect_Src_List[i].volume = m_EffectVolume;
        }
    }

    public void PlayEffectSound(string a_FileName)
    {
        AudioClip a_AudioClip = null;
        if (AudioClipDict.ContainsKey(a_FileName) == true)
        {
            a_AudioClip = AudioClipDict[a_FileName] as AudioClip;
        }
        else
        {
            a_AudioClip = Resources.Load("Art/Sound/Effect/" + a_FileName) as AudioClip;
            AudioClipDict.Add(a_FileName, a_AudioClip);
        }

        if(m_Effect_Obj_List.Count < Effect_Max_Count)
        {
            GameObject newSoundObj = new GameObject();
            newSoundObj.name = "SoundEffectObj";
            newSoundObj.transform.SetParent(GameManager.staticSoundParent.transform);
            newSoundObj.transform.localPosition = Vector3.zero;

            AudioSource a_AudioSrc = newSoundObj.AddComponent<AudioSource>();
            a_AudioSrc.playOnAwake = false;
            a_AudioSrc.loop = false;

            m_Effect_Src_List[m_Effect_Obj_List.Count] = a_AudioSrc;
            m_Effect_Obj_List.Add(newSoundObj);
        }

        if(a_AudioClip != null && m_Effect_Src_List[m_Effect_Index] != null)
        {
            m_Effect_Src_List[m_Effect_Index].PlayOneShot(a_AudioClip, m_EffectVolume);

            m_Effect_Index++;
            if (Effect_Max_Count <= m_Effect_Index)
                m_Effect_Index = 0;
        }
    }

    public void Init()
    {

    }

    public void OnUpdate()
    {
        
    }

    public void Clear()
    {

    }


}
