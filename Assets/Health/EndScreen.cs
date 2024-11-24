using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EndScreen : MonoBehaviour
{
    public List<GameObject> objectsToDisable; // Lista de objetos para desactivar
    public VideoPlayer videoPlayer; // Referencia al componente VideoPlayer
    
    public MeshRenderer meshRenderer;//Mesh Renderer
    public Camera mainCamera; // Referencia a la cámara principal

    void Start()
    {
        // Asegúrate de obtener la cámara principal si no se ha asignado
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        videoPlayer.enabled = false;
        meshRenderer.enabled = false;
    }

    void Update()
    {
        // Alinea el video con la cámara
        if (videoPlayer != null && mainCamera != null)
        {
            AlignVideoWithCamera();
        }
    }

    // Desactiva todos los objetos de la lista
    public void DeactivateObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    // Reproduce el video y asegura que esté alineado con la cámara
    public void PlayVideo()
    {
        if (videoPlayer != null && meshRenderer!=null)
        {
            meshRenderer.enabled = true;
            videoPlayer.enabled = true;
            videoPlayer.Play();
            AlignVideoWithCamera(); // Alinea el video al inicio
        }
    }

    // Alinea el video con la cámara
    private void AlignVideoWithCamera()
    {
        // Actualiza la posición y rotación del video para que coincida con la cámara
        transform.position = mainCamera.transform.position + mainCamera.transform.forward * 2f; // Ajusta la distancia desde la cámara
        transform.rotation = mainCamera.transform.rotation;
    }
}
