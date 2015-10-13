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

	[Header("Prefabs")]
	public BonusController DivideBonus;

	[HideInInspector]
	public bool FixedGame;
	#endregion
	#region Private
	private static Parameters _instance;
	#endregion
	#endregion

	#region Methods
	#region Public
	/// <summary>
	/// Получить префаб бонуса по типу бонуса. Возвращает NULL, если для типа бонуса нету
	/// префаба.
	/// </summary>
	/// <param name="bonus">Тип бонуса.</param>
	/// <returns>Префаб бонуса. Может вернуть NULL.</returns>
	public GameObject GetPrefabBonus(BonusType bonus)
	{
		switch (bonus)
		{
			case BonusType.Divide:
				return DivideBonus.gameObject;
		}
		return null;
	}
	#endregion
	#region Private
	private void Awake()
	{
		_instance = this;
	}
	#endregion
	#endregion
}