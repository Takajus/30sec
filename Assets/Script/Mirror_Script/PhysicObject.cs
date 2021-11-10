using UnityEngine;

public class PhysicObject : MonoBehaviour
{
    private float speed = 5f;
    private Rigidbody rb;
    public Transform target;
    private Vector3 mouvement;
    private float dist;
    private float startDist;
    private bool fire = false;

    void Awake()
    {
        fire = false;
        if (gameObject.GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();
        }
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            letsGo();
        }

        //transform.LookAt(target.position);
    }

    void FixedUpdate()
    {
        if (fire)
        {
            dist = Vector3.Distance(transform.position, target.position);
            if (dist > 25f)
            {
                dist = 25f;
            }
            mouvement = (target.position - transform.position).normalized;
            mouvement = (mouvement * (speed + startDist)) + new Vector3(0, dist / 2, 0);
            //transform.LookAt(mouvement - transform.position);
            transform.rotation = Quaternion.LookRotation(mouvement);
            rb.velocity = mouvement;
            if (dist <= startDist / 20)
            {
                annulation();
            }
        }
    }

    void OnCollisionEnter(Collision quiMeTouche)
    {
        if (fire)
        {
            //AkSoundEngine.PostEvent("Play_collision_rockvsrock", gameObject);
            annulation();
        }
    }

    void letsGo()
    {
        startDist = Vector3.Distance(transform.position, target.position);
        if (startDist > 25f)
        {
            startDist = 25f;
        }
        fire = true;
        rb.isKinematic = false;
    }

    void annulation()
    {
        fire = false;
        rb.useGravity = true;
        Destroy(GetComponent<PhysicObject>());
    }
}