using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    public static UnityEvent onGoalTrigger;

	private void Awake()
	{
		if (onGoalTrigger == null)
			onGoalTrigger = new UnityEvent();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		onGoalTrigger.Invoke();
	}
}
