using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomArrow : MonoBehaviour
{

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private LayerMask _Enemy;

    [Range(0f, 1f)]
    [SerializeField] private float _bounciness;
    [SerializeField] private bool _bUseGravity;

    [SerializeField] private int _maxCollisions;
    [SerializeField] private float _maxLifeTime;

    private int _collisions;
    PhysicMaterial _physics_mat;

    private void Start()
    {
        SetUp();
    }

    private void Update()
    {
        if (_collisions >= _maxCollisions) Destroy(gameObject);

        _maxLifeTime -= Time.deltaTime;
        //if (_maxLifeTime <= 0) Destroy(gameObject);
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        // stop moving
        *//*if (collision.collider.CompareTag("Enemy")) Destroy(gameObject);
        else Destroy(gameObject);*//*
        Destroy(gameObject);
    }*/

    private void SetUp()
    {
        // Create Physic Material
        _physics_mat = new PhysicMaterial();
        _physics_mat.bounciness = _bounciness;
        _physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        _physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        // Assign material to collider
        GetComponent<SphereCollider>().material = _physics_mat;

        // Set Gravity
        _rb.useGravity = _bUseGravity;
    }
}
