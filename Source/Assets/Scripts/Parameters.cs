using UnityEngine;
using System.Collections;

public class Parameters : MonoBehaviour
{
	#region Properties
	#region Public
	public static Parameters Instance
	{
		get
		{
			if (_instance == null)
				_instance = FindObjectOfType<Parameters>();
			if (_instance == null)
				_instance = new Parameters();
			return _instance;
		}
	}

	[HideInInspector]
	public bool FixedGame;
	#endregion
	#region Private
	private static Parameters _instance;
	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void Awake()
	{
		_instance = this;
	}
	#endregion
	#endregion
}