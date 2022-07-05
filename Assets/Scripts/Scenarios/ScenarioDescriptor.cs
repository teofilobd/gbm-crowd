using UnityEngine;

public abstract class ScenarioDescriptor : ScriptableObject
{
	public abstract void SetupScenario(ScenarioController scenarioController);
}
