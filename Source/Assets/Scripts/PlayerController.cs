using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	#region Properties
	#region Public
	public static PlayerController Instance;

	public GameObject GameOverText;
	public Text Score;
	public float Factor = 0.3f;
	#endregion
	#region Private
	private const float MAX_X = 3.75f;

	public int _countBlock;
	private int _score = 0;
	#endregion
	#endregion

	#region Methods
	#region Public
	public void AddScore(int value)
	{
		_score += value;
		Score.text = _score.ToString();
    }

	public void DestroyBlock()
	{
		_countBlock--;
    }
	#endregion
	#region Private
	private void Awake()
	{
		Instance = this;
		_countBlock = GameObject.FindGameObjectsWithTag("Block").Length;
    }

	private void Update()
	{
		var pos = transform.position;
		pos.x += Input.GetAxis("Horizontal") * Factor;
		if (Mathf.Abs(pos.x) > MAX_X)
			pos.x = ((pos.x < 0) ? -1 : 1) * MAX_X;
        transform.position = pos;

		if (_countBlock == 0)
			GameOver();
    }

	private void GameOver()
	{
		Time.timeScale = 0;
		GameOverText.SetActive(true);
    }
	#endregion
	#endregion
}