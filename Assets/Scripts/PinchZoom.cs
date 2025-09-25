using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch; // Se importa el paquete para el soporte de gestos multitouch
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
// Alias para usar touch directamente desde enhancedtouch y no confundir con unityengine.touch

[RequireComponent(typeof(Camera))] // Este script exige en el gameobject es para que tenga una camara 

public class PinchZoom : MonoBehaviour
{
    public float zoomSpeed = 0.15f; // Velocidad de respuesta del zoom (que tan rapido cambia)
    public float fovMin = 30f; // Valor minimo de field of view (camara en perspectiva)
    public float fovMax = 75f; // Valor maximo de field of view (camara en perspectiva)
    public float orthoMin = 3f; // Valor manimo para camaras ortograficas (usadas en 2D)
    public float orthMax = 20f; // Valor maximo para camaras ortograficas 

    private Camera cam; // Referencia a la camara en la que se hace zoom
    private float? lastDistance; // Guardia la distancia anterior entre los dos dedos

    void OnEnable()
    {
        cam = GetComponent<Camera>();
        // Obtenemos la camara adjunta al gameobject

        EnhancedTouchSupport.Enable();
        // Activamos el sistema de enhancedtouch para detectar multiples dedos
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        // Cuando se desactiva el script, apagamos el soporte de multitouch
    }

    void Update()
    {
        var touches = Touch.activeTouches;
        // Obtenemos todos los toques activos en la pantalla

        if (touches.Count < 2)
        {
            // Si hay menos de dos dedos, reseteamos la distancia previa y salimos
            lastDistance = null;
            return;
        }

        // Si hay al menos dos dedos, tomamos las posiciones de los dos primeros
        Vector2 p0 = touches[0].screenPosition;
        Vector2 p1 = touches[1].screenPosition;

        float currentDistance = Vector2.Distance(p0, p1);
        // Calculamos la distancia actual entre los dedos

        if (lastDistance.HasValue)
        {
            float delta = currentDistance - lastDistance.Value;
            // Calculamos cuanto cambio la distancia desde el ultimo frame
            // Positivo si los dedos se separan, negativo si se acercan

            float zoomAmount = -delta * zoomSpeed * Time.deltaTime;
            // Ajustamos el signo para que pellizcar signifique acercar

            if (cam.orthographic)
            {
                // si la camara es ortografica 2D, modificamos el tamaño ortografico
                cam.orthographicSize = Mathf.Clamp(
                    cam.orthographicSize + zoomAmount, orthoMin, orthMax
                );
            }
            else
            {
                // Si la camara es de perspectiva 3D, modificamos el field of view
                cam.fieldOfView = Mathf.Clamp(
                    cam.fieldOfView + zoomAmount * 10f, fovMin, fovMax
                );
            }
        }

        // Guardamos la distancia actual como referencia para el proximo frame
        lastDistance = currentDistance;
    }
}
