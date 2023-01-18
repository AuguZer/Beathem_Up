using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMove : MonoBehaviour
{
    public enum RotationControls
    {
        MOUSE,
        KEYBOARD
    }


    [SerializeField] RotationControls controls;


    Vector2 dirMove;


    Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        float dirY = Input.GetAxisRaw("Vertical");

        dirMove = new Vector2(dirX, dirY).normalized;

        //transform.Translate(dirMove * Time.deltaTime * 5f, Space.World);
        rb2d.velocity = dirMove * 5f;

        if(Input.GetKeyDown(KeyCode.P))
        {
            switch (controls)
            {
                case RotationControls.MOUSE:

                    controls = RotationControls.KEYBOARD;
                    break;
                case RotationControls.KEYBOARD:
                    controls = RotationControls.MOUSE;
                    break;
                default:
                    break;
            }
        }


        switch (controls)
        {
            case RotationControls.MOUSE:
                // SOURIS
                Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);

                Vector3 dirToMouse = Input.mousePosition - playerScreenPos;

                if (dirToMouse.x != 0)
                {
                    transform.eulerAngles = new Vector3(0, dirToMouse.x > 0 ? 0 : 180, 0);

                }

                break;
            case RotationControls.KEYBOARD:

                float dirRot = 0;

                if (Input.GetKey(KeyCode.K) && !Input.GetKey(KeyCode.J))
                {
                    dirRot = 1;
                }

                if (Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.K))
                {
                    dirRot = -1;
                }

                if (dirRot != 0)
                {
                    transform.eulerAngles = new Vector3(0, dirRot > 0 ? 0 : 180, 0);

                }
                break;
            default:
                break;
        }




        





    }
}
