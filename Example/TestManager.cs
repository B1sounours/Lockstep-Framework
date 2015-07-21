using UnityEngine;
using System.Collections;
using Lockstep;
public class TestManager : MonoBehaviour {
	void Start () {

		LockstepManager.Initialize ();
		AgentController controller = AgentController.Create ();
		for (int i = 0; i < 256; i++)
			controller.CreateAgent (AgentCode.Footman);
		PlayerManager.AddAgentController (controller);
	}

	void FixedUpdate () {
		LockstepManager.Simulate ();
	}

	void Update ()
	{
		LockstepManager.Visualize ();
	}
}
