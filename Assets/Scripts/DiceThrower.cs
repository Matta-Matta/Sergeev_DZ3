using UnityEngine;
using UnityEngine.InputSystem;

public class DiceThrower : MonoBehaviour
{
    [Header("Кубики")]
    public Rigidbody[] diceRigidbodies;   // Сюда перетащим все кубики

    [Header("Настройки силы броска")]
    public float minForce = 6f;
    public float maxForce = 12f;
    public float minTorque = 5f;
    public float maxTorque = 15f;

    [Header("Ограничение броска")]
    public float throwCooldown = 1.5f;    // Минимальное время между бросками

    private InputSystem_Actions inputActions;
    private float lastThrowTime;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Gameplay.Throw.performed += OnThrow;   // Подписываемся на нажатие
    }

    private void OnDisable()
    {
        inputActions.Gameplay.Throw.performed -= OnThrow;
        inputActions.Disable();
    }

    private void OnThrow(InputAction.CallbackContext context)
    {
        // Не даём бросать слишком часто
        if (Time.time - lastThrowTime < throwCooldown)
            return;

        ThrowDice();
        lastThrowTime = Time.time;
    }

    private void ThrowDice()
    {
        foreach (Rigidbody rb in diceRigidbodies)
        {
            if (rb == null) continue;

            // Сбрасываем текущую скорость
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Случайная сила (в основном вверх + немного в стороны)
            Vector3 force = new Vector3(
                Random.Range(-3f, 3f),
                Random.Range(minForce, maxForce),
                Random.Range(-3f, 3f)
            );

            // Случайное вращение
            Vector3 torque = new Vector3(
                Random.Range(minTorque, maxTorque),
                Random.Range(minTorque, maxTorque),
                Random.Range(minTorque, maxTorque)
            );

            rb.AddForce(force, ForceMode.Impulse);
            rb.AddTorque(torque, ForceMode.Impulse);
        }
    }
}