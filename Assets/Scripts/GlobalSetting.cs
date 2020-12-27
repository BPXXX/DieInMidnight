using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSetting 
{

    private static GlobalSetting instance = new GlobalSetting();
    private float bgmVolume=1.0f;
    public static GlobalSetting Instance
    {
        get {return  instance; }
    }
    public float BGMVolume {
        get { return bgmVolume; }
        set { bgmVolume = value; }
    
    }
}
