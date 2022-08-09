using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LSJ_MouseCursorUI : MonoBehaviour
{
    public RectTransform transform_cursor;
    // Start is called before the first frame update
    private void Start()
    {
        Init_Cursor();
    }
    private void Update()
    {
        Update_MousePosition();
        Debug.Log(Input.mousePosition);
    }

    private void Init_Cursor()
    {
        Cursor.visible = false;
        transform_cursor.pivot = Vector2.up;

        if (transform_cursor.GetComponent<Graphic>())
            transform_cursor.GetComponent<Graphic>().raycastTarget = false;
    }
    private void Update_MousePosition()
    {
        Vector2 mousePos = Input.mousePosition;
        transform_cursor.position = mousePos;
    }
}
