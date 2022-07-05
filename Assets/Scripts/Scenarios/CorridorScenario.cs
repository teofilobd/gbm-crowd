using UnityEngine;

[CreateAssetMenu(fileName = "Corridor_Scenario", menuName = "Crowd/Scenarios/Corridor")]
public class CorridorScenario : OppositeScenario 
{
	public float CorridorWidth = 10.0f;
	public float CorridorLength = 7.0f;

	public override void SetupScenario(ScenarioController scenarioController) 
	{
		//base.Rows = (int) (CorridorLength / base.SpaceBetweenAgents);

		base.SetupScenario (scenarioController);

		scenarioController.AddWall(new Vector3(-CorridorWidth/2.0f, 0.0f, -CorridorLength/2.0f),
								   new Vector3( CorridorWidth/2.0f, 0.0f, -CorridorLength/2.0f), 0.3f);
		scenarioController.AddWall(new Vector3(-CorridorWidth/2.0f, 0.0f,  CorridorLength/2.0f),
								   new Vector3( CorridorWidth/2.0f, 0.0f,  CorridorLength/2.0f), 0.3f);
	}
}
