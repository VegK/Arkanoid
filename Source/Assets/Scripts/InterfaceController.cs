using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour
{
	private const string FINAL_SCORE = "Финальный счёт: ";

	#region Properties
	#region Public
	public static InterfaceController Instance
	{
		get
		{
			if (_instance == null)
				_instance = FindObjectOfType<InterfaceController>();
			return _instance;
		}
	}

	[Header("Счёт")]
	public GameObject PanelScore;
	public Text Score;

	[Header("Жизни")]
	public GameObject PanelLife;
	public Text Life;

	[Header("Конец игры")]
	public GameObject PanelGameOver;
	public Text FinalScore;
	#endregion
	#region Private
	private static InterfaceController _instance;
	#endregion
	#endregion

	#region Methods
	#region Public
	public void OnClickRestart()
	{
		Time.timeScale = 1;
		Application.LoadLevel(Application.loadedLevel);
	}
	/// <summary>
	/// Изменить счёт.
	/// </summary>
	/// <param name="value">Значение.</param>
	public void SetScore(int value)
	{
		Score.text = value.ToString();
	}
	/// <summary>
	/// Изменить количество оставшихся жизней.
	/// </summary>
	/// <param name="value">Значение.</param>
	public void SetLife(int value)
	{
		Life.text = value.ToString();
	}
	/// <summary>
	/// Вывести экран конеца игры.
	/// </summary>
	/// <param name="finalScore">Финальный счёт.</param>
	public void GameOver(int finalScore)
	{
		Time.timeScale = 0;
		PanelScore.SetActive(false);
		PanelLife.SetActive(false);
        FinalScore.text = FINAL_SCORE + finalScore;
		PanelGameOver.SetActive(true);
	}
	#endregion
	#region Private
	private void Awake()
	{
		_instance = this;
		GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
	}
	#endregion
	#endregion
}