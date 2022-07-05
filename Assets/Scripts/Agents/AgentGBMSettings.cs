using UnityEngine;

[CreateAssetMenu(fileName = "AgentGBM_Settings", menuName = "Crowd/Agents Settings/GBM")]
public class AgentGBMSettings : AgentSettings
{
    public float SigmaDca = 0.3f;
    public float SigmaTtca = 2.0f;
    public float SigmaAlpha = 2.0f;
    public float SigmaSpeed = 3.0f;
}
