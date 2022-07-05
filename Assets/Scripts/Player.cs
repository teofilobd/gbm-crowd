using UnityEngine;

public class Player : MonoBehaviour 
{
	public float Speed = 1.0f;
	public float RotationSpeed = 100.0f;
	public Renderer Renderer;
	private Vector3 m_InitialScale;

	void Start () 
	{
		m_InitialScale = transform.localScale;
	}
	
	void Update() 
	{
		float translation = Input.GetAxis("Vertical") * Speed;
		float rotation = Input.GetAxis("Horizontal") * RotationSpeed;
		translation *= Time.deltaTime;
		rotation *= Time.deltaTime;

		Vector3 scaleFactors = new Vector3 (0.0f,0.0f,0.0f);
		float scaleStep = 0.3f;

		if (Input.GetKey (KeyCode.R)) 
			transform.localScale = m_InitialScale;
		if(Input.GetKey(KeyCode.Z)) {
			if(Input.GetKey(KeyCode.LeftShift)) 
				scaleFactors.z = scaleStep;
			else 
				scaleFactors.z = -scaleStep;
		}
		if(Input.GetKey(KeyCode.X)) {
			if(Input.GetKey(KeyCode.LeftShift)) 
				scaleFactors.x = scaleStep;
			else 
				scaleFactors.x = -scaleStep;
		}

		transform.localScale = new Vector3 (transform.localScale.x + scaleFactors.x,
		                                    transform.localScale.y,
		                                    transform.localScale.z + scaleFactors.z);

		Vector3 previousPosition = transform.position;
		transform.Translate(translation, 0, 0);
		Vector3 newPosition = transform.position;

		transform.Rotate(0, rotation, 0);
	
		Vector3 velocity = (newPosition - previousPosition) / Time.deltaTime;

		Renderer.material.SetVector("_ObstacleVelocity", new Vector4(velocity.x,velocity.y,velocity.z));
		Renderer.material.SetFloat("_SigmaTtca", 2.0f);
		Renderer.material.SetFloat("_SigmaDca", 0.3f);
		Renderer.material.SetFloat("_ObstacleID", 1.0f);
	}
}
