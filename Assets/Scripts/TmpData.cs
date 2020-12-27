using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpData : MonoBehaviour
{
    private static TmpData instance = new TmpData();
    public static TmpData Instanace {
        get { return instance; } }
    // Start is called before the first frame update
    public string CurrentBGM { get; set; }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
