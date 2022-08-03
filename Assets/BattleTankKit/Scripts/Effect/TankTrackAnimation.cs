using UnityEngine;
using System.Collections;

// 무한궤도 효과
public class TankTrackAnimation : MonoBehaviour
{
	Renderer render;
	public Vector2 UVDirection = new Vector2(1, 0);
	public float speed = 1;

	void Start()
	{
		render = GetComponent<Renderer>();
	}

	public void MoveTrack(Vector2 vector)
	{
		if (render == null || render.material == null)
			return;

		Vector2 moveVector = Vector2.zero;

		if(UVDirection.x != 0)
			moveVector.x = vector.x * UVDirection.x;
			
		if(UVDirection.y != 0)
			moveVector.y = vector.x * UVDirection.y;
			
		render.material.mainTextureOffset += moveVector * speed * Time.deltaTime;
	}
}
