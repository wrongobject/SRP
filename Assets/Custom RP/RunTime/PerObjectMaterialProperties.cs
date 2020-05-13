#pragma warning disable 0414
using UnityEngine;

public class PerObjectMaterialProperties : MonoBehaviour
{
    static int baseColorId = Shader.PropertyToID("_BaseColor"),
               cutoffId = Shader.PropertyToID("_Cutoff"),
               metallicId = Shader.PropertyToID("_Metallic"),
               smoothnessId = Shader.PropertyToID("_Smoothness");

    MaterialPropertyBlock block;
    [SerializeField, Range(0f, 1f)]
    public float alphaCutoff = 0.5f, metallic = 0f, smoothness = 0.5f;
    public Color baseColor;
    public float cutOff;
    private void Awake()
    {
        block = new MaterialPropertyBlock();
        
    }

    private void OnValidate()
    {
        block.SetFloat(cutoffId, cutOff);
        block.SetVector(baseColorId, baseColor);
        block.SetFloat(metallicId, metallic);
        block.SetFloat(smoothnessId, smoothness);
        GetComponent<Renderer>().SetPropertyBlock(block);
    }
}

