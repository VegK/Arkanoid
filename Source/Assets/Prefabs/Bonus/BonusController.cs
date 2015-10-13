using UnityEngine;
using System.Collections;

public class BonusController : MonoBehaviour
{
	#region Properties
	#region Public
	public GameObject MainBonusObject;

	public BonusType Bonus;
	#endregion
	#region Private

	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void Update()
	{
		transform.Translate(Vector3.down * Time.deltaTime * 2);
		MainBonusObject.transform.Rotate(Vector3.right * Time.deltaTime * 300);
	}
	#endregion
	#endregion
}