using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status_Check : MonoBehaviour
{
    public GameObject objectFalse; // Objeto que representa el estado "False"
    public GameObject objectTrue;  // Objeto que representa el estado "True"

    // Start is called before the first frame update
    void Start()
    {
        // Inicialmente establece el estado (opcional)
        SetFalse(); // Llama a SetFalse por defecto si es necesario
    }

    // Actualiza el estado a "False"
    public void SetFalse()
    {
        if (objectFalse != null)
        {
            objectFalse.SetActive(true);  // Activa el objeto que indica "False"
        }

        if (objectTrue != null)
        {
            objectTrue.SetActive(false); // Desactiva el objeto que indica "True"
        }
    }

    // Actualiza el estado a "True"
    public void SetTrue()
    {
        if (objectTrue != null)
        {
            objectTrue.SetActive(true);  // Activa el objeto que indica "True"
        }

        if (objectFalse != null)
        {
            objectFalse.SetActive(false); // Desactiva el objeto que indica "False"
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Aquí puedes agregar lógica adicional si es necesario
    }
}
