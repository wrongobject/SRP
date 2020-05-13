using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AutoChildPosition : MonoBehaviour
{
    public int col = 2;
    public int row = 2;
    public float space = 1;
    public int childCount = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (col <= 0) col = 1;
        if (row <= 0) row = 1;
        int count = transform.childCount;

        if (count <= 0) return;
        GameObject temp = transform.GetChild(0).gameObject;
        if (count < childCount)
        {
            for (int i = count; i < childCount; i++)
            {
                GameObject item = GameObject.Instantiate(temp);
                item.transform.SetParent(temp.transform.parent);
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.identity;
            }
        }

        count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform childTrans = transform.GetChild(i);
            float offsetX = i % 2 == 0 ? space * 0.5f : 0;
            childTrans.localPosition = new Vector3(i / col * space + offsetX,0, i % row * space);
            childTrans.name = "Obj" + i;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
