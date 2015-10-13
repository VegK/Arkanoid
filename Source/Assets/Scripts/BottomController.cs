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
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Ball")
		{
			if (!BallController.DestroyBall(other.gameObject))
				PlayerController.Instance.Life--;
        }
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Ball")
		{
			other.GetComponent<Rigidbody>().isKinematic = true;
		}
		else if (other.gameObject.tag == "Bonus")
		{
			Destroy(other.gameObject);
		}
	}
	#endregion
	#endregion
}