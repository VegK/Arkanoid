using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{
	#region Properties
	#region Public
	public int HitPoint = 1;
	public int Score = 1;
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
		HitPoint--;
		if (HitPoint == 0)
		{
			PlayerController.Instance.Score += Score;
			PlayerController.Instance.DestroyBlock();
			Destroy(gameObject);
		}
	}
	#endregion
	#endregion
}