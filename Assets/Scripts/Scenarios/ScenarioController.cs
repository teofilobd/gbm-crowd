using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScenarioController : MonoBehaviour
{
	public List<ScenarioDescriptor> ScenarioDescriptors;
	public List<GameObject> AgentTypes;
	public uint SelectedScenario = 0;
	public uint SelectedAgentType = 0;
	private int m_AgentCount = 0;

	private void Start()
	{
		LoadScenario();
	}

	public void LoadScenario()
	{
		DestroyAgents();

		if (SelectedScenario < ScenarioDescriptors.Count)
		{
			ScenarioDescriptor scenarioDescriptor = ScenarioDescriptors[(int)SelectedScenario];
			if (scenarioDescriptor != null)
			{
				scenarioDescriptor.SetupScenario(this);
			}
		}
	}

	private void DestroyAgents()
	{
		for (int childID = transform.childCount - 1; childID >= 0; --childID)
		{
			Transform agent = transform.GetChild(childID);
			if (agent != null)
			{
				DestroyImmediate(agent.gameObject);
			}
		}
		m_AgentCount = 0;
		GradientBasedModel.ClearAgents();
		PureReactiveModel.ClearAgents();
	}

	public void AddAgent(Vector3 position, Vector3 goalPosition, Color color)
	{
		GameObject agent = (GameObject)Instantiate(AgentTypes[(int)SelectedAgentType]);
		agent.name = "Agent_" + m_AgentCount.ToString();
		m_AgentCount++;

		agent.transform.position = position;
		
		AgentBase agentScript = agent.GetComponent<AgentBase>();
		agentScript.GoalPosition = goalPosition;
		agentScript.GoalPosition.y += agent.transform.localScale.y; // Height, fix this later.

		agentScript.Velocity = (goalPosition - position).normalized * agentScript.Settings.ComfortSpeed;

		agent.transform.LookAt(agentScript.GoalPosition);
		agent.transform.Rotate(Vector3.up, -90.0f);
		agentScript.GoalPosition.y = 0.0f;

		agentScript.Color = color;
		
		agent.SetActive(true);

		agent.transform.parent = transform;

		if (agentScript is AgentGBM agentGBM)
		{
			GradientBasedModel.AddAgent(agentGBM);
		}

		if (agentScript is AgentPRM agentPRM)
		{
			PureReactiveModel.AddAgent(agentPRM);
		}
	}

	public void AddStaticCylinder(Vector3 position, float height, float radius)
	{
		GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		cylinder.name = "Cylinder";

		cylinder.transform.localScale = new Vector3(cylinder.transform.localScale.x * radius,
													cylinder.transform.localScale.y * height,
													cylinder.transform.localScale.z * radius);
		cylinder.transform.position = new Vector3(position.x,
												  position.y + cylinder.transform.localScale.y,
												  position.z);

		cylinder.SetActive(true);
		cylinder.AddComponent<Obstacle>();
		cylinder.AddComponent<BoxCollider>();

		cylinder.GetComponent<Renderer>().sharedMaterial.shader = Shader.Find("Custom/ObstacleShader");

		cylinder.transform.parent = transform;
	}

	public void AddStaticBox(Vector3 position, Vector3 orientation, Vector3 boxDimensions)
	{
		GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
		box.name = "Box";

		box.transform.localScale = new Vector3(box.transform.localScale.x * boxDimensions.x,
											   box.transform.localScale.y * boxDimensions.z,
											   box.transform.localScale.z * boxDimensions.y);

		box.transform.position = new Vector3(position.x,
												 position.y + box.transform.localScale.y / 2.0f,
											  position.z);

		box.SetActive(true);
		box.AddComponent<Obstacle>();
		box.AddComponent<BoxCollider>();
		box.GetComponent<Renderer>().sharedMaterial.shader = Shader.Find("Custom/ObstacleShader");

		box.transform.rotation = Quaternion.FromToRotation(new Vector3(1, 0, 0), orientation);

		box.transform.parent = transform;
	}

	public void AddWall(Vector3 start, Vector3 end, float width)
	{
		Vector3 boxDimensions = new Vector3((start - end).magnitude, width, 4.0f);

		AddStaticBox((start + end) / 2.0f, (end - start).normalized, boxDimensions);
	}

	public void SetScenario(int scenarioId)
	{
		SelectedScenario = (uint) scenarioId;
		LoadScenario();
	}

	public void SetAgentType(int agentType)
    {
		SelectedAgentType = (uint) agentType;
		LoadScenario();
    }

    private void OnValidate()
    {
        if(SelectedScenario >= ScenarioDescriptors.Count)
        {
			SelectedScenario = (uint) ScenarioDescriptors.Count - 1;
        }

		if(SelectedAgentType >= AgentTypes.Count)
        {
			SelectedAgentType = (uint)AgentTypes.Count - 1;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ScenarioController))]
public class ScenarioControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
		
		ScenarioController scenarioController = target as ScenarioController;

        if(GUILayout.Button("Load Scenario"))
        {
			scenarioController.LoadScenario();
        }
    }
}
#endif
