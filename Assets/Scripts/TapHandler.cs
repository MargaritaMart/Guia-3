using UnityEngine;
using UnityEngine.InputSystem;
// Importamos UnityEngine para trabajar en unity
// e InputSystem para usar el nuevo sistema de entrada toques, botones, etc.

public class TapHandler : MonoBehaviour
{
    [Header("Arrastra desde el .inputactions")]
    public InputActionReference tapAction;
    // Accion que detecta el toqe. se configura en el asset de input ations

    public InputActionReference pointerPositionAction;
    // Accion que nos da la posicion del dedo o mouse en la pantalla

    [Header("Opcional")]
    public Camera cam;
    // Camara a usar para convertir la posicion de pantalla a un rayo
    // Si no se asigna, se usara Camera.main

    public LayerMask raycastLayers = ~0;
    // Permite filtrar en que capas queremos detectar el toque
    // ~0 significa en todas las capas

    void OnEnable()
    {
        // Se ejecuta cuando el objeta se activa en la escena
        if (tapAction != null)
        {
            tapAction.action.performed += OnTap;
            // Nos suscribimos al evento performed para ejecutar el metodo OnTap
            tapAction.action.Enable();
            // Habilitamos la accion para que empiece a detectar toques
        }
        if (pointerPositionAction != null)
        {
            pointerPositionAction.action.Enable();
            // Habilitamos la accion para que empiece a detectar la posicion del dedo
        }
    }

    void OnDisable()
    {
        // Se ejecuta cuando el objeta se desactiva en la escena
        if (tapAction != null)
        {
            tapAction.action.performed -= OnTap;
            // Nos desuscribimos del evento para evitar errores o fugas de memoria
            tapAction.action.Disable();
            // Deshabilitamos la accion para que deje de detectar toques y liberar recursos
        }
        if (pointerPositionAction != null)
        {
            pointerPositionAction.action.Disable();
            // Deshabilitamos la accion para que deje de detectar la posicion del dedo
        }
    }

    private void OnTap(InputAction.CallbackContext context)
    {
        // Este metodo se ejecuta cada vez que ocurre un tap en la pantalla

        Camera cameraToUse = cam != null ? cam : Camera.main;
        // Usamos la camara asignada, si es nula o si es la camara principal

        if (cameraToUse == null) return;
        // Si no hay camara disponible salimos sin hacer nada

        Vector2 screenPosition = pointerPositionAction.action.ReadValue<Vector2>();
        // Leemos la posicion del toque en coordenadas de pantalla pixeles

        Ray ray = cameraToUse.ScreenPointToRay(screenPosition);
        // Creamos un rayo desde la camara hacia donde el usurario toco

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, raycastLayers))
        {
            // Si el rayo golpea un objeto en la escena dentro de 1000 unidades y es las capas permitidas:

            Debug.Log($"Tap sobre: {hit.collider.gameObject.name}");
            // Mostramos en consola el nombre del objeto tocado

            // Opcional
            // GameObjet.CratePrimitive(PrimitiveType.Sphere).transform.position = hit.point;
        }
        else
        {
            // Si el toque no golpeo nada en la escena:
            Debug.Log("Tap en vacio (No golpeo ningun objeto)"); 
        }
    }
}
