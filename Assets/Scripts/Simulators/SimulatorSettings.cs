using UnityEngine;

[CreateAssetMenu(fileName = "Simulator_Settings", menuName = "Crowd/Simulator Settings")]
public class SimulatorSettings : ScriptableObject
{
	public int VisionWidth = 256;   // Vision width.
	public int VisionHeight = 48;    // Vision height.
	public float SightTiltAngle = 40;    // Tilt angle.
	public Shader VisionShader;      // Shader used to process the NPC's visual data.
}
