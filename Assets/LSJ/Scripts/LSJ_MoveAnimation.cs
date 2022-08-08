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
    public static LSJ_MoveAnimation Instance;
    private void Awake()
    {
        Instance = this;
    }
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
        // if (playermove.isMove == false)
        //    mat.mainTextureOffset += Vector2.zero * animSpeed * Time.deltaTime;
        MoveAnim();

        // LSJ_PlayerMove playermove = player.GetComponent<LSJ_PlayerMove>();
        //mat.mainTextureOffset += Vector2.left * animSpeed * Time.deltaTime;
    }

    public  void MoveAnim()
    {
        float offset = Time.time * scrollSpeed * Input.GetAxisRaw("Vertical");
        renderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        renderer.material.SetTextureOffset("_BumpMap", new Vector2(offset, 0));
    }

    public void MoveAnim2()
    {
        float offset = Time.time * scrollSpeed;
        renderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        renderer.material.SetTextureOffset("_BumpMap", new Vector2(offset, 0));
    }
}
