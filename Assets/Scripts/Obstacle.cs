using UnityEngine;

public class Obstacle : MonoBehaviour 
{
	private Renderer m_Renderer;

	void Start () 
	{
		gameObject.layer = LayerMask.NameToLayer("Obstacle");
		m_Renderer = GetComponent<Renderer> ();
		
		m_Renderer.material.SetVector("_ObstacleVelocity", new Vector4(0,0,0));
		m_Renderer.material.SetFloat("_SigmaTtca", 2.0f);
		m_Renderer.material.SetFloat("_SigmaDca", 0.3f);
		m_Renderer.material.SetFloat("_ObstacleID", 1.0f);
	}
}
