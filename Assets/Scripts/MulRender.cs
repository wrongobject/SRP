using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MulRender : MonoBehaviour
{
    static int baseColorId = Shader.PropertyToID("_BaseColor");
    static int cutOff = Shader.PropertyToID("_CutOff");
    static int maintexST = Shader.PropertyToID("_MainTex_ST");
    public int count;
   
    public float range = 1;
    public Mesh mesh = default;
    public Material material = default;
    public Matrix4x4[] matrix;
    private MaterialPropertyBlock block;
    private List<Vector4> colorList = new List<Vector4>();
    private List<Vector4> offList = new List<Vector4>();
    private List<float> cutOffList = new List<float>();
    private void OnEnable()
    {
        
    }

    private void OnValidate()
    {
        if (count <= 0) count = 1;
       

        if (block == null)
        {
            block = new MaterialPropertyBlock();
        }
       
        block.Clear();
        colorList.Clear();
        offList.Clear();
        cutOffList.Clear();
        matrix = new Matrix4x4[count];
        for (int i = 0; i < count; i++)
        {
            
            matrix[i] = Matrix4x4.TRS((transform.position + Random.insideUnitSphere * range),Quaternion.identity,Vector3.one);
            colorList.Add(new Vector4(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1));
            offList.Add(new Vector4(Random.Range(0.1f, 5f), Random.Range(0.1f, 5f), Random.Range(0.1f, 1f), Random.Range(0.1f, 1f)));
            cutOffList.Add(Random.Range(0.0f, 1));
        }
        block.SetFloatArray(cutOff, cutOffList);        
        block.SetVectorArray(maintexST, offList);
        block.SetVectorArray(baseColorId,colorList);
    }

    // Update is called once per frame
    void Update()
    {
        if (!mesh) return;
        if (!material) return;
        if (matrix == null) return;
        if (block == null) return;

        Graphics.DrawMeshInstanced(mesh,0, material, matrix,count,block);
    }

    void UpdatePosition()
    {
        //Todo 旋转效果
    }

    void UpdateMatrix()
    {
        //Todo 旋转效果
    }
}
