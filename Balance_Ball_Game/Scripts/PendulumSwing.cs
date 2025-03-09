using System;
using UnityEngine;

[RequireComponent(typeof(HingeJoint2D))]
public class PendulumSwing : MonoBehaviour
{
    [SerializeField] private HingeJoint2D hingJoint;
    [SerializeField] private Transform ballTransform;
    [SerializeField] private float swingSpeed = 100f;
    [SerializeField] private float maxAngle = 45f; // Максимальный угол отклонения
    private GameObject attachedCircle;
    private JointMotor2D motor;
    private bool isSwingingRight = true;
    private Rigidbody2D rigidbd;
    public event Action<GameObject> OnCircleReleased; // Событие сброса круга

    private void Awake()
    {
        rigidbd = GetComponent<Rigidbody2D>();
        motor = hingJoint.motor;
        motor.motorSpeed = swingSpeed;
        hingJoint.motor = motor;
    }

    private void FixedUpdate()
    {
        // Автоматическое изменение направления при достижении угла
        float currentAngle = Mathf.Abs(rigidbd.rotation);
        if (currentAngle >= maxAngle)
        {
            ReverseDirection();
        }
    }

    private void ReverseDirection()
    {
        isSwingingRight = !isSwingingRight;
        motor.motorSpeed = isSwingingRight ? swingSpeed : -swingSpeed;
        hingJoint.motor = motor;
    }

    // Метод для сброса круга (вызывается по нажатию)
    public void ReleaseCircle()
    {
        // Логика открепления и активации физики для круга
        if (attachedCircle == null) return;

        Rigidbody2D circleRb = attachedCircle.AddComponent<Rigidbody2D>();
        circleRb.gravityScale = 1f;
        circleRb.simulated = true;
        attachedCircle.transform.SetParent(null);
        OnCircleReleased?.Invoke(attachedCircle); // Уведомляем о сбросе
        attachedCircle = null;
    }
   
    public void AttachCircle(GameObject circle)
    {
        attachedCircle = circle;
        attachedCircle.transform.position = ballTransform.position;
        attachedCircle.transform.SetParent(transform);
    }
    public bool HasCircle() => attachedCircle != null;
    
}
