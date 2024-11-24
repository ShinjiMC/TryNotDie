using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorObject : MonoBehaviour
{
    private Renderer rendererObject;

    public Color defaultColor;
    public Color newColor;
    private bool colorChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        rendererObject = GetComponent<Renderer>();
        rendererObject.material.color = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (colorChanged)
            {
                ChangeColorMethod(defaultColor);  // Cambia a default si ya cambió antes
            }
            else
            {
                ChangeColorMethod(newColor);  // Cambia a newColor si está en el default
            }
        }
    }

    // Cambia el color del objeto
    public void ChangeColorMethod(Color colorToChange)
    {
        colorChanged = !colorChanged;  // Alterna entre true y false
        rendererObject.material.color = colorToChange;  // Aplica el color
    }
}