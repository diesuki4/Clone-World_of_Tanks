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

    public bool isMoving;
    public float animSpeed = 1.0f;
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        var MeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        mat = MeshRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
            mat.mainTextureOffset += Vector2.left * animSpeed * Time.deltaTime;
    }
}
