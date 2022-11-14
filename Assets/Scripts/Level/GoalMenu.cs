using UnityEngine;

public class GoalMenu : MonoBehaviour
{
    [SerializeField] private GameObject goalMenu;

	private void Start()
	{
		Goal.onGoalTrigger.AddListener(DisplayGoalMenu);
		Player.OnPlayerDeath.AddListener(DisplayGoalMenu);
	}

	public void DisplayGoalMenu()
	{
		goalMenu.SetActive(true);
	}
}
