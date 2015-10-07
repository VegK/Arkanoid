using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{
	#region Properties
	#region Public
	public static BallController Instance;

	public Vector2 StartSpeed = new Vector2(170f, 200f);
	#endregion
	#region Private
	private Vector3 _startPosition;
	private bool _peace = true;
	private Rigidbody _rigidbody;
	#endregion
	#endregion

	#region Methods
	#region Public
	public void Reset()
	{
		_rigidbody.isKinematic = true;
		transform.position = _startPosition;
		_peace = true;
    }
	#endregion
	#region Private
	private void Awake()
	{
		Instance = this;
		_rigidbody = GetComponent<Rigidbody>();
		_startPosition = transform.position;
    }

	private void Update()
	{
		if (_peace && Input.GetKey(KeyCode.Space))
		{
			_peace = false;
			_rigidbody.isKinematic = false;
			_rigidbody.AddForce(StartSpeed);
		}

		if (_peace)
		{
			var pos = transform.position;
			pos.x = PlayerController.Instance.transform.position.x;
			transform.position = pos;
		}
	}
	#endregion
	#endregion
}