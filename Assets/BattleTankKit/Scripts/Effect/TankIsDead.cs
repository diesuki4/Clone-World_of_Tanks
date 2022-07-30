using UnityEngine;
using System.Collections;
using HWRWeaponSystem;

public class TankIsDead : MonoBehaviour
{
	[Header("적을 죽였을 때 올라가는 점수")]
	public int ScorePlus = 1;

	void Start() { }

	// Tank가 죽은 후에 1번 호출된다.
	public void OnDead()
	{
		if (gameObject.GetComponent<Tank>())
			if (TankGame.TankGameManager != null &&
				TankGame.TankGameManager.TankControl != null &&
				TankGame.TankGameManager.TankControl.TargetTank != null)
				// 자신을 마지막으로 맞춘 탱크가 플레이어 탱크이면
				if (gameObject.GetComponent<Tank>().LatestHit == TankGame.TankGameManager.TankControl.TargetTank.gameObject)
					// 플레이어 점수를 증가
					TankGame.PlayerScore += ScorePlus;
	}
}
