using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefrenceTest : MonoBehaviour
{
    ClassA a;
    ClassB b;
    int genarationA = -1;
    int genarationB = -1;
    int gcGenaration = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        GUILayout.Label("当前GC最大代数：" + System.GC.MaxGeneration);
        if (a)
        {            
            GUILayout.Label("a 的代数：" + genarationA);
        }
        if (b)
        {            
            GUILayout.Label("a 的代数：" + genarationB);
        }

        if (GUILayout.Button("添加ClassA"))
        {
            a = gameObject.AddComponent<ClassA>();
            genarationA = System.GC.GetGeneration(a);
        }

        if (GUILayout.Button("添加ClassB"))
        {
            b = gameObject.AddComponent<ClassB>();
            genarationB = System.GC.GetGeneration(b);
        }

        if (GUILayout.Button("删除ClassA"))
        {
            if (a)
            {
                GameObject.DestroyImmediate(a);
            }
        }

        if (GUILayout.Button("删除ClassB"))
        {
            if (b)
            {
                GameObject.DestroyImmediate(b);
            }
        }
        gcGenaration =  UnityEditor.EditorGUILayout.IntField("GC目标代数：",gcGenaration);
        if (GUILayout.Button("GC"))
        {
            if (gcGenaration >= 0)
                System.GC.Collect(gcGenaration);
            else
            {
                Debug.LogError("error genaration number");
            }
        }
    }
}
