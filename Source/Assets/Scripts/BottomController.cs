using UnityEngine;
using System.Collections;

public class BottomController : MonoBehaviour
{
	#region Properties
	#region Public
	
	#endregion
	#region Private

	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Ball")
			BallController.Instance.Reset();
	}
	#endregion
	#endregion
}