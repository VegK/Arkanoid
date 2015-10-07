using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public const float MAX_X = 3.75f;

	#region Properties
	#region Public
	public static PlayerController Instance;

	public float FactorMove = 0.3f;
	public int Score
	{
		get
		{
			return _score;
		}
		set
		{
			_score = value;
			InterfaceController.Instance.SetScore(_score);
		}
	}
	public int Life
	{
		get
		{
			return _life;
		}
		set
		{
			if (value < 0)
				GameOver();
			_life = value;
			InterfaceController.Instance.SetLife(_life);
		}
	}
	#endregion
	#region Private
	private int _countBlock;
	private int _score;
	[SerializeField]
	private int _life = 3;
	#endregion
	#endregion

	#region Methods
	#region Public
	/// <summary>
	/// Уменьшаем количество блоков и проверяем конец игры.
	/// </summary>
	public void DestroyBlock()
	{
		_countBlock--;
		if (_countBlock <= 0)
			GameOver();
	}
	#endregion
	#region Private
	private void Awake()
	{
		Instance = this;

		_countBlock = GameObject.FindGameObjectsWithTag("Block").Length;
		if (_countBlock <= 0)
			GameOver();
    }
	private void Start()
	{
		InterfaceController.Instance.SetScore(Score);
		InterfaceController.Instance.SetLife(Life);
	}
	private void Update()
	{
		var pos = transform.position;
		pos.x += Input.GetAxis("Horizontal") * FactorMove;
		if (Mathf.Abs(pos.x) > MAX_X)
			pos.x = ((pos.x < 0) ? -1 : 1) * MAX_X;
		transform.position = pos;
	}
	/// <summary>
	/// Остновить игру и вывести на экран панель GameOver.
	/// </summary>
	private void GameOver()
	{
		InterfaceController.Instance.GameOver(_score);
    }
	#endregion
	#endregion
}