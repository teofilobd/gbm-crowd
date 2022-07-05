using UnityEngine;

[CreateAssetMenu(fileName = "Crossing2_Scenario", menuName = "Crowd/Scenarios/Crossing2")]
public class Crossing2Scenario : ScenarioDescriptor 
{
	// Scenario properties.
	public int Rows = 3;
	public int Columns = 3;
	public float SpaceBetweenAgents = 2.0f;
	public float DistanceFromCenter = 2.0f;
	
	// Configure a scenario in this function.
	public override void SetupScenario(ScenarioController scenarioController) 
	{
		float space = SpaceBetweenAgents;
		float aux = ((Rows - 1) * SpaceBetweenAgents)/2.0f;
		Vector3 initial = new Vector3(-DistanceFromCenter, 0.0f, -aux);
		Vector3 initial2 = new Vector3(-aux, 0.0f, DistanceFromCenter);
		
		for (int rowIndex = 0 ; rowIndex < Rows; rowIndex++) 
		{
			for (int columnIndex = 0; columnIndex < Columns; columnIndex++) 
			{
				Vector3 agentPosition = new Vector3(initial.x + columnIndex * space, 
				                                    0.0f, 
				                                    initial.z + rowIndex * space);
				Vector3 goalPosition = agentPosition;
				goalPosition.x = DistanceFromCenter - (Columns-columnIndex-1) * space;
				scenarioController.AddAgent(agentPosition, goalPosition, new Color(0.6f,0.9f,0.9f));
				scenarioController.AddAgent(goalPosition, agentPosition, new Color(0.9f,0.6f,0.9f));
				
				agentPosition = new Vector3(initial2.x + rowIndex * space, 0.0f, initial2.z - columnIndex * space);
				goalPosition = agentPosition;
				goalPosition.z = -DistanceFromCenter + (Columns-columnIndex-1) * space;
				scenarioController.AddAgent(agentPosition, goalPosition, new Color(0.9f,0.5f,0.5f));
				scenarioController.AddAgent(goalPosition, agentPosition, new Color(0.9f,0.9f,0.5f));
			}
		}
	}
}
