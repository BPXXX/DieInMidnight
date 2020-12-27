using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Utlis : MonoBehaviour
{
    private static Utlis instance = new Utlis();
    public static Utlis Instance {
        get { return instance; }
        set { instance = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static AudioSource LoadAudioSource(AudioSourceType type,string filename)
    {
        string Audiotype = " ";
        if(string.IsNullOrEmpty(filename))
        {
            Debug.Log("Loading AudioSource Failed!");
            return null;
        }
        if(type==AudioSourceType.BGM)
        {
            Audiotype = "BGM";
        }
        else if(type==AudioSourceType.SE)
        {
            Audiotype = "SE";
        }
        string filepath = string.Format("Sounds/{0}/{1}", Audiotype, filename);
        AudioSource audioSource = Resources.Load<AudioSource>(filename);
        return audioSource;
    }
}
