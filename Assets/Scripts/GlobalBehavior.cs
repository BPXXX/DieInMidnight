using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBehavior:MonoBehaviour
{

    private static GlobalBehavior instance = new GlobalBehavior();
    AudioSource BGMAudioSource;
    AudioSource SEAudioSource;
    static object _lock = new object();
    static bool isDestroy = false;
    private float curVolume = 1.0f;
    public static GlobalBehavior Instance
    {
        get
        {

            lock (_lock)
            {
                if (instance == null && isDestroy != true)
                {
                    var go = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/GlobalController"));
                    go.name = "GlobalController";
                    DontDestroyOnLoad(go);

                }
                return instance;
            }
        }
    }
    public float CurVolume
    {
        get { return curVolume; }
    }
    public void SetCurVolume(float volume)
    {
        curVolume = volume;
    }
    private void Awake()
    {
        
    }
    public void playBGM(string fileName,bool loop=true)
    {
        if(TmpData.Instanace.CurrentBGM==fileName&&BGMAudioSource.isPlaying)
        {
            return;
        }
      if(string.IsNullOrEmpty(fileName))
        {
            StartCoroutine("FadeBgm",null);
            return;
        }
      else
        {
            var clip = Utlis.LoadAudioSource(AudioSourceType.BGM,fileName);
            StartCoroutine("FadeBgm",clip);
            TmpData.Instanace.CurrentBGM = fileName;
        }


    }
    IEnumerator FadeBgm(AudioClip next)
    {
        AudioClip orgin = BGMAudioSource.clip;
        const float DUR = 1.0f;
        float dur = DUR;

        if (orgin != null && BGMAudioSource.isPlaying)
        {
            while (dur > 0f)
            {
                dur -= Time.deltaTime;
                BGMAudioSource.volume = dur / DUR * GlobalSetting.Instance.BGMVolume;
                yield return null;
            }
        }
        dur = DUR;
        if (next != null)
        {
            BGMAudioSource.clip = next;
            BGMAudioSource.Play();
            while (dur > 0f)
            {
                dur -= Time.deltaTime;
                BGMAudioSource.volume = (DUR - dur / DUR) * GlobalSetting.Instance.BGMVolume;
                yield return null;
            }
        }
        else
        {
            BGMAudioSource.Stop();
        }
        
    }
    public void playSE(string fileName, bool loop = false)
    {
        SEAudioSource.loop = loop;
        if (fileName == null)
        {
            SEAudioSource.Stop();
        }
        else
        {
            string filePath = string.Format("Sound/SE/{0}", fileName);
            var clip = Resources.Load<AudioClip>(filePath);
            SEAudioSource.clip = clip;
            SEAudioSource.loop = loop;
            SEAudioSource.Play();
        }
    }
    public void onBGMVolumeChange(float volume )
    {

    }
}
