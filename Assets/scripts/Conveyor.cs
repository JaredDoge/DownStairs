using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    // Start is called before the first frame update

    // GameObject currentGameObj;
    [SerializeField] float vel=3;
    void Start()
    {
      
    }

    // Update is called once per frame

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.contacts[0].normal == new Vector2(0f, 1f))
        {

            switch (other.gameObject.tag)
            {
                case "ConveyorRight":
                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.right * vel;
                    break;
                case "ConveyorLeft":
                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.left * vel;
                    break;
            }
        }

    }

}
