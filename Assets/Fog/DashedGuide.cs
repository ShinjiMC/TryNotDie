using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashedLineRenderer : MonoBehaviour
{
    public Transform targetPoint;  // Punto objetivo al cual debe apuntar la línea
    public LineRenderer lineRenderer;  // Referencia al componente LineRenderer
    public float dashLength = 0.1f;  // Longitud de cada segmento de la línea
    public float gapLength = 0.2f;   // Longitud del espacio entre segmentos
    public float lineWidth = 0.01f;  // Ancho de la línea
    public Color lineColor = Color.white; // Color de la línea, modificable por el usuario

    void Start()
    {
        // Configurar el grosor de la línea
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // Asegúrate de que el LineRenderer tiene al menos 2 posiciones
        lineRenderer.positionCount = 2;

        // Asignar el material y el color desde la variable pública
        Material lineMaterial = new Material(Shader.Find("Unlit/Color"));
        lineMaterial.color = lineColor;  // Línea del color que se haya asignado
        lineRenderer.material = lineMaterial;
    }

    void Update()
    {
        DrawDashedLine(transform.position, targetPoint.position);
    }

    void DrawDashedLine(Vector3 startPoint, Vector3 endPoint)
    {
        // Calcula la distancia total entre el punto inicial y el punto final
        float totalDistance = Vector3.Distance(startPoint, endPoint);

        // Crea una lista para los puntos de la línea
        List<Vector3> points = new List<Vector3>();

        Vector3 direction = (endPoint - startPoint).normalized;  // Dirección de la línea
        float currentDistance = 0f;  // Rastrea la distancia recorrida en la línea

        // Alternar entre los segmentos de línea (dashed) y los espacios (gap)
        bool drawingDash = true;

        while (currentDistance < totalDistance)
        {
            if (drawingDash)
            {
                // Agrega el punto inicial del segmento de línea
                points.Add(startPoint + direction * currentDistance);

                // Calcula la distancia al final del segmento de línea
                currentDistance += dashLength;

                // Asegúrate de no exceder la distancia total
                if (currentDistance > totalDistance)
                    currentDistance = totalDistance;

                // Agrega el punto final del segmento de línea
                points.Add(startPoint + direction * currentDistance);
            }
            else
            {
                // Saltar la distancia del gap
                currentDistance += gapLength;
            }

            // Alternar entre dash y gap
            drawingDash = !drawingDash;
        }

        // Actualiza los puntos del LineRenderer
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}
