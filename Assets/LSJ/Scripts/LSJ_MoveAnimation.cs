using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 탱크의 무한궤도 애니메이션
// Texture의 offset x 값을 바꿔준다
// MeshRenderer에서 가져옴
// 필요 속성 : 속도, 머터리얼
public class LSJ_MoveAnimation : MonoBehaviour
{
    // private float scrollSpeed = 1.0f;
    // private Renderer _renderer;

    public float animSpeed = 1.0f;
    Material mat;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        var MeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        mat = MeshRenderer.material;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        LSJ_PlayerMove playermove = player.GetComponent<LSJ_PlayerMove>();

        // if (playermove.isMove == false)
        //    mat.mainTextureOffset += Vector2.zero * animSpeed * Time.deltaTime;

        if (playermove.isMove)
            mat.mainTextureOffset += Vector2.left * animSpeed * Time.deltaTime;
    }
}
