using UnityEngine;
//using Mirror;

public class ObjectTest : MonoBehaviour //NetworkBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private bool _isFloating;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private Vector3 posOffset = new Vector3();
    private Vector3 tempPos = new Vector3();

    private void Start()
    {
        _isFloating = false;
        tempPos = transform.position;
        posOffset = transform.position;
    }

    private void Update()
    {
        if (_isFloating)
        {
            posOffset = transform.position;

            tempPos = posOffset + Vector3.up / 2;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.position = tempPos;
            //PosUpdate(tempPos);
        }
    }

    public void Launching(Vector3 _powerDirection)
    {
        transform.position = _powerDirection;
    }

    public void Floating(bool isFloating)
    {
        _isFloating = isFloating;
    }

    public void PosUpdate(Vector3 _tempPos)
    {
        transform.position = _tempPos;
    }
}
