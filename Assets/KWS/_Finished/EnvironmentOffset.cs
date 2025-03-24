using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentOffset : MonoBehaviour
{
    private List<float> oceanWaveSpeed = new List<float>
    {
        0.2f, 0.8f, 1f
    };
    private List<float> oceanWaveHeight = new List<float>
    {
        0.2f, 0.5f, 0.8f
    };

    private List<Color32> skyboxTint = new List<Color32>
    {
        new Color32(140, 140, 140, 255),
        new Color32(242, 255, 0, 255),
        new Color32(0, 0, 0, 255)
    };
    private List<Color32> skyboxGround = new List<Color32>
    {
        new Color32(255, 255, 255, 255),
        new Color32(255, 117, 0, 255),
        new Color32(166, 100, 0, 255)
    };
    private List<float> skyboxExposure = new List<float> 
    { 
        1.3f, 0.5f, 0.25f
    };

    private List<Vector3> dLightRotation = new List<Vector3>
    {
        new Vector3(50, -30, 0),
        new Vector3(20, -30, 0),
        new Vector3(10, -30, 0)
    };
    private List<Color32> dLightEmmision = new List<Color32>
    {
        new Color32(255, 255, 255, 255),
        new Color32(174, 174, 174, 255),
        new Color32(114, 114, 114, 255)
    };


    [SerializeField] private WaveEffect waveScript;
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private Light directionalLight;


    public int currStage = 0;


    public void SetStageTheme(int stage)
    {
        waveScript.waveSpeed = oceanWaveSpeed[stage - 1];
        waveScript.waveHeight = oceanWaveHeight[stage - 1];

        skyboxMaterial.SetColor("_Tint", skyboxTint[stage - 1]);
        skyboxMaterial.SetColor("_Ground", skyboxGround[stage - 1]);
        skyboxMaterial.SetFloat("_Exposure", skyboxExposure[stage - 1]);
            
        directionalLight.transform.eulerAngles = dLightRotation[stage - 1];
        directionalLight.color = dLightEmmision[stage - 1]; 
    }


    // TEST Key Mapping
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        SetStageTheme(currStage);
    //        currStage++;
    //        currStage %= 3;
    //    }
    //}
}
