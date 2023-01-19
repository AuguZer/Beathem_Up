using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimJanoLamp : MonoBehaviour
{

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey)
        {
            animator.SetBool("FALLBOOL", true);
        }
    }
}
