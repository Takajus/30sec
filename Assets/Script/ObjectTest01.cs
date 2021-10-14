using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTest01 : MonoBehaviourPunCallbacks
{
    private PhotonView PV;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private bool _isFloating;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private Vector3 posOffset = new Vector3();
    private Vector3 tempPos = new Vector3();

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        _isFloating = false;
        posOffset = transform.position;
    }

    private void Update()
    {
        
        
    }

    public void Launching(Vector3 _powerDirection)
    {
        transform.localPosition = _powerDirection;
    }

    public void Floating(bool isFloating)
    {
        _isFloating = isFloating;
        if(_isFloating)
            posOffset = transform.position;
    }

    [PunRPC]
    void IsFloating()
    {
        if (_isFloating)
        {
            tempPos = posOffset + Vector3.up / 2;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.position = tempPos;
        }
    }
}
