using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private GameObject cam;

    private Vector3 velocity;
    private Vector3 rotation;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    private Vector3 thrusterVelocity;

    [SerializeField] private float cameraRotationLimit = 85f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    #region Methode pour récup valeur

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotateCamera(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }

    public void ApplyThruster(Vector3 _thrusterVeloticy)
    {
        thrusterVelocity = _thrusterVeloticy;
    }

    #endregion

    private void FixedUpdate() // pour la physique
    {
        PerformMovement();
        PerformRotation();
    }

    public void PerformMovement() 
    {
        if(velocity != Vector3.zero)
        {
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }

        if(thrusterVelocity != Vector3.zero)
        {
            //rb.AddForce(thrusterVelocity * Time.fixedDeltaTime, ForceMode.Impulse);
            rb.velocity = new Vector3(rb.velocity.x, thrusterVelocity.y, rb.velocity.z);
            thrusterVelocity = Vector3.zero;
        }
    }

    private void PerformRotation()
    {
        // Calcul de rotation de la Caméra
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        // Application du calcul
        cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

}
