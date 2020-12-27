using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MapManagerListener 
{
    void ClearMaps();
    void GenerateMaps();
    void ChangeWeather(MapWeather weather);
}
