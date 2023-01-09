using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] public GameObject player;

    [SerializeField] float speed = 5f;
    [SerializeField] float damage = 5f;
    public float currenthealth;
    Rigidbody rb;
    Vector3 dirToMove;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (player == null)
        {
            return;
        }
        //CALCUL DE LA DIRECTION VERS LE PLAYER
        dirToMove = player.transform.position - transform.position;

        //APPLIQUE LA VITESSE AU RIGIDBODY
        rb.velocity = dirToMove.normalized * speed;
        // APPLIQUE LA ROTATION 
        transform.LookAt(player.transform);

        Vector3 FixPlayerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(player.transform);

    }
    private void OnCollisionEnter(Collision collision)
    {
        // Collision avec le player
        if (collision.gameObject.tag == "Player")

        {
            // JE DETRUIS LE PLAYER
            collision.gameObject.GetComponent<PlayerHealth>().playerCurrentHealth -= damage;


            // DETRUIRE L'ENNEMI
            Destroy(gameObject);

        }
    }


}
