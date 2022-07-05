using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public ScenarioController ScenarioController;
    public TMP_Dropdown ScenarioDropdown;
    public TMP_Dropdown AgentTypeDropdown;

    void Start()
    {
        ScenarioDropdown.ClearOptions();

        List<string> options = new List<string>();
        foreach (ScenarioDescriptor scenarioDescriptor in ScenarioController.ScenarioDescriptors)
        {
            options.Add(scenarioDescriptor.name);
        }

        ScenarioDropdown.AddOptions(options);

        ScenarioDropdown.onValueChanged.AddListener(OnScenarioSelected);

        AgentTypeDropdown.ClearOptions();
        options.Clear();
        foreach(GameObject agentTypeGO in ScenarioController.AgentTypes)
        {
            options.Add(agentTypeGO.name);
        }

        AgentTypeDropdown.AddOptions(options);

        AgentTypeDropdown.onValueChanged.AddListener(OnAgentTypeSelected);
    }

    void OnScenarioSelected(int newScenario)
    {
        ScenarioController.SetScenario(newScenario);
    }

    void OnAgentTypeSelected(int newAgentType)
    {
        ScenarioController.SetAgentType(newAgentType);
    }
}
