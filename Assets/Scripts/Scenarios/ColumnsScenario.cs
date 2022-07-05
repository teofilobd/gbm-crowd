using UnityEngine;

[CreateAssetMenu(fileName = "Columns_Scenario", menuName = "Crowd/Scenarios/Columns")]
public class ColumnsScenario : OppositeScenario 
{
	// Scenario properties.
	public int RowsOfColumns    = 3;
	public int ColumnsOfColumns = 2;
	public float SpaceBetweenColumnsInX = 3.0f;
	public float SpaceBetweenColumnsInY = 4.0f;
	public float ColumnsRadius = 1.0f;

	// Configure a scenario in this function.
	public override void SetupScenario(ScenarioController scenarioController) 
	{
		base.SetupScenario (scenarioController);

		Vector3 initial = new Vector3 (-(ColumnsOfColumns - 1.0f) * SpaceBetweenColumnsInX / 2.0f,
									   0.0f,
		                               -(RowsOfColumns - 1.0f) * SpaceBetweenColumnsInY / 2.0f);

		for (int rowIndex = 0 ; rowIndex < RowsOfColumns; rowIndex++)
		{
			for (int columnIndex = 0; columnIndex < ColumnsOfColumns; columnIndex++)
			{
				scenarioController.AddStaticCylinder(new Vector3(initial.x + columnIndex * SpaceBetweenColumnsInX, 
													0.0f, 
													initial.z + rowIndex * SpaceBetweenColumnsInY), 2.0f, ColumnsRadius);
			}
		}
	}
}
