using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾� ��ũ�� ���ѱ˵� �ִϸ��̼�
// Texture�� offset x ���� �ٲ��ش�
// MeshRenderer���� ������
// �ʿ� �Ӽ� : �ӵ�, ���͸���
public class LSJ_MoveAnimation : MonoBehaviour
{
    private float scrollSpeed = 0.3f;
    private Renderer renderer;

    // public float animSpeed = 1.0f;
    // Material mat;
    // GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();   
        // var MeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        // mat = MeshRenderer.material;
        // player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float offset = Time.time * scrollSpeed * Input.GetAxisRaw("Vertical");
        renderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        renderer.material.SetTextureOffset("_BumpMap", new Vector2(offset, 0));
        // if (playermove.isMove == false)
        //    mat.mainTextureOffset += Vector2.zero * animSpeed * Time.deltaTime;


        // LSJ_PlayerMove playermove = player.GetComponent<LSJ_PlayerMove>();
        //mat.mainTextureOffset += Vector2.left * animSpeed * Time.deltaTime;
    }
}
