using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾� ��ũ�� ���ѱ˵� �ִϸ��̼�
// Texture�� offset x ���� �ٲ��ش�
// MeshRenderer���� ������
// �ʿ� �Ӽ� : �ӵ�, ���͸���
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
