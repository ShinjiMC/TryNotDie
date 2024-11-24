using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;  // Necesario si estás usando XR Origin

public class SyncColliderWithCamera : MonoBehaviour
{
    public Transform cameraTransform;  // La cámara del XR Origin
    public CapsuleCollider capsuleCollider;

    void Update()
    {
        // Sincronizar la posición del Capsule Collider con la cámara, pero mantener la altura y el radio correctos
        Vector3 newPos = cameraTransform.position;
        newPos.y = capsuleCollider.bounds.center.y;  // Mantener la altura del Capsule Collider
        capsuleCollider.center = transform.InverseTransformPoint(newPos);
    }
}
