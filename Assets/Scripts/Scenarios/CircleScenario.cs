using UnityEngine;

[CreateAssetMenu(fileName = "Circle_Scenario", menuName = "Crowd/Scenarios/Circle")]
public class CircleScenario : ScenarioDescriptor 
{
	// Scenario properties.
	public int NumberOfAgents = 1;
	public float CircleRadius = 5.0f;

	// Configure a scenario in this function.
	public override void SetupScenario(ScenarioController scenarioController) 
	{
		float angle = Mathf.Rad2Deg * Mathf.PI / (NumberOfAgents / 2.0f);
		Vector3 agentPosition = new Vector3 (CircleRadius, 0.0f, 0.0f);
		Vector3 goalPosition = -agentPosition;

		for (int i = 0; i<NumberOfAgents; i++) 
		{
			scenarioController.AddAgent(agentPosition, goalPosition, Color.white);

			agentPosition = Quaternion.AngleAxis(angle, Vector3.up) * agentPosition;
			goalPosition = -agentPosition;
		}
	}
}
