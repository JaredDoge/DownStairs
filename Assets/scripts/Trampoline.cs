using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : Floor
{
    // Start is called before the first frame update


    //Animator animator;
    void Start()
    {
        // animator=GetComponent<Anim>

    }

   

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
          //  other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
        }
    }
}
