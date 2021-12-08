using UnityEngine;

public class DeathAnim : MonoBehaviour
{
    private float deathDistance;
    public Animator anim;
    private int deathPosition;
    private Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision truc)
    {
        if (truc.gameObject.tag == "Item")
        {
            jeSuisMort(truc.transform);
        }
    }

    private void OnTriggerEnter(Collider truc)
    {
        if (truc.tag == "Item")
        {
            jeSuisMort(truc.transform);
        }
    }

    private void jeSuisMort(Transform killer)
    {

        // en face
        deathDistance = Vector3.Distance(killer.position, transform.position + transform.forward);
        deathPosition = 0;

        // droite
        if (Vector3.Distance(killer.position, transform.position + transform.right) < deathDistance)
        {
            deathDistance = Vector3.Distance(killer.position, transform.position + transform.right);
            deathPosition = 1;
        }
        // derrière
        if (Vector3.Distance(killer.position, transform.position - transform.forward) < deathDistance)
        {
            deathDistance = Vector3.Distance(killer.position, transform.position - transform.forward);
            deathPosition = 2;
        }
        // gauche
        if (Vector3.Distance(killer.position, transform.position - transform.right) < deathDistance)
        {
            deathDistance = Vector3.Distance(killer.position, transform.position - transform.right);
            deathPosition = 3;
        }

        anim.SetBool("dead", true);
        anim.SetInteger("deathPosition", deathPosition);
        col.enabled = false;
    }
}
