/// <summary>
/// Типы бонусов.
/// </summary>
public enum BonusType
{
	None,
	/// <summary>
	/// Расширять - увеличивает платформу в 2 раза.
	/// </summary>
	Expand,
	/// <summary>
	/// Делить - появляется два дополнительных шарика.
	/// </summary>
	Divide,
	/// <summary>
	/// Лазер.
	/// </summary>
	Laser,
	/// <summary>
	/// Медленный - замедление всех шариков.
	/// </summary>
	Slow,
	/// <summary>
	/// Прорыв.
	/// </summary>
	Break,
	/// <summary>
	/// Ловить - шарики прилепают к платформе.
	/// </summary>
	Catch,
	/// <summary>
	/// Игрок - дополнительная жизнь.
	/// </summary>
	Player
}