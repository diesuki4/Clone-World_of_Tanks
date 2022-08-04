using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무한궤도 효과
public class CKB_TrackMover : MonoBehaviour
{
    // 방향
    public enum Direction
    {
        // 정방향
        Normal = 1,
        // 역방향
        Inverse = -1
    }
    [Header("정방향/역방향")]
    public Direction directon = Direction.Normal;

    [Header("속력")]
	public float speed = 1;

	Material mat;
	Vector2 UVDirection;

	void Start()
	{
		mat = GetComponent<Renderer>().material;
        UVDirection = new Vector2((float)directon, 0);
	}

	void Update() { }

    // 무한궤도를 돌린다.
	public void MoveTrack(Vector2 vector)
	{
		if (!mat)
			return;

		Vector2 moveVector = Vector2.zero;

		if(UVDirection.x != 0)
			moveVector.x = vector.x * UVDirection.x;
			
		if(UVDirection.y != 0)
			moveVector.y = vector.x * UVDirection.y;
			
		mat.mainTextureOffset += moveVector * speed * Time.deltaTime;
	}
}
