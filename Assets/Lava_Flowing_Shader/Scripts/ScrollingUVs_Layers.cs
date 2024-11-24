using UnityEngine;
using System.Collections;

public class ScrollingUVs_Layers : MonoBehaviour
{
    public Vector2 uvAnimationRate = new Vector2(1.0f, 0.0f);
    public Texture texture;  // Textura asignada por referencia pública

    private Vector2 uvOffset = Vector2.zero;
    private Material material;

    void Start()
    {
        // Obtener el material del renderer
        material = GetComponent<Renderer>().sharedMaterial;

        // Asignar la textura si se ha definido
        if (texture != null)
        {
            material.mainTexture = texture;
        }
    }

    void LateUpdate()
    {
        uvOffset += (uvAnimationRate * Time.deltaTime);
        if (GetComponent<Renderer>().enabled && material != null)
        {
            material.SetTextureOffset("_MainTex", uvOffset);
        }
    }
}
