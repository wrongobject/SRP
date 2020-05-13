using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class PerObjectProperty : MonoBehaviour
{
    static int baseColorId = Shader.PropertyToID("_BaseColor");
    [SerializeField]
    Color baseColor = Color.white;
    static MaterialPropertyBlock block;
    // Start is called before the first frame update
    void Awake()
    {
        OnValidate();
    }

    private void OnEnable()
    {
        baseColor = new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f),1);
        OnValidate();
    }

    void OnValidate()
    {
        if (block == null)
        {
            block = new MaterialPropertyBlock();
        }
        block.SetColor(baseColorId, baseColor);
        GetComponent<Renderer>().SetPropertyBlock(block);
    }

   
}
