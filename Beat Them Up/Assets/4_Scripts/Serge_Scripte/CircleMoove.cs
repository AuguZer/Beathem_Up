using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMoove : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] bool compass;
    CapsuleCollider2D capsCol2d;
    Rigidbody2D rigBod2d;
    Vector2 basicMove;
    [SerializeField] string basicsens;
   
    // Start is called before the first frame update
    void Start()
    {
        rigBod2d = GetComponent<Rigidbody2D>();
        capsCol2d = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        basicMove = new Vector2(transform.position.x, transform.position.y).normalized * speed * Time.deltaTime;

       
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector2(-1, 0).normalized * speed * Time.deltaTime);
            basicsens = "left";
           // if (Input.GetKey(KeyCode.LeftArrow))
            // {
               // transform.rotation = Quaternion.Euler(0, 180, 0);
            // }
            
        }

        if (Input.GetKey(KeyCode.Z))
        {
            transform.Translate(new Vector2(1, 0).normalized * speed * Time.deltaTime);
            //transform.LookAt((Vector2.right).normalized * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.X))
        {
            transform.Translate(new Vector2(0, -1).normalized * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector2(0, 1).normalized * speed * Time.deltaTime);
        }

        


    }
}
