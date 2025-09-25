using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [Header("Accion Move (Vector2) desde el .inputactions")]
    public InputActionReference moveAction;
    // Referencia a la accion Move que se configuro en el asset de input actions
    // Devuelve un vector2 con la direccion (-1..1 en x e y)

    [Header("Movimiento")]
    public float speed = 3.5f;
    // Velocidad de movimiento del objeto

    public bool moveInCameraPlane = true;
    // Si es true, el movimiento se calcula relativo a la camara principal (util para juegos en 3ra persona)

    void OnEnable()
    {
        // Cuando el objetivo se activa, habilitamos la accion para que empiece a recibir valores
        if (moveAction != null) moveAction.action.Enable();
    }

    void OnDisable()
    {
        // Cuando el objetivo se desactiva, deshabilitamos la accion para que empiece a recibir valores
        if (moveAction != null) moveAction.action.Disable();
    }

    void Update()
    {
        // Este metodo se llama cada frame
        if (moveAction == null) return; // Si no hay accion asignada, salimos

        Vector2 input = moveAction.action.ReadValue<Vector2>();
        // Leemos el valor de la accion move (ejes X e Y del joystick o stick virtual)

        Vector3 dir = new Vector3(input.x, 0f, input.y);
        // Conertimos el vector2 en un vector3 para mover en el plano XZ (horizontal)

        if (moveInCameraPlane && Camera.main)
        {
            // Si queremos que el movimiento sea relativo a la camara principal:

            // Direccion de la camara en el plano horizontal (ignoramos en el eje Y)
            Vector3 camFwd = Camera.main.transform.forward;
            camFwd.y = 0f; camFwd.Normalize();

            Vector3 camRight = Camera.main.transform.right;
            camRight.y = 0f; camRight.Normalize();

            // Ajustamos la direccion del movimiento segun la orientacion de la camara
            dir = camFwd * dir.z + camRight * dir.x;
        }

        // Movemos el objeto multiplicado por la velocidad y el tiempo entre frames (para suavidad)
        transform.position += dir * speed * Time.deltaTime;
    }
}
