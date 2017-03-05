using UnityEngine;
using System.Collections;
using DaiMangou.RadarBuilder3D;
/// <summary>
/// With this script we will control out radars scenescale value. 
/// </summary>
public class UIRadarSliderController : MonoBehaviour
{

    [TextArea(10, 100)]
    public string Info = " ";
    public _3DRadar _3DRadar_;




    public void valueChange(float value)
    {
        // controls how muh of the blips we can see at any one time
        if (_3DRadar_)
            _3DRadar_.RadarDesign.SceneScale = value;

    }




}
