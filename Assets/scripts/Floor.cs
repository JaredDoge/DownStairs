using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{

    public static float moveSpeed = 1f;
    // Start is called before the first frame update
     void Start()
    {

    }

    // Update is called once per frame
    protected void Update()
    {
        //https://blog.csdn.net/qq_39131023/article/details/106908664
        /**
        Space.World坐标系是固定的方向，不会随着物体的旋转而改变。
        Space.Self坐标系会随着自身的旋转而改变。
        **/    
       // transform.Translate(0, moveSpeed * Time.deltaTime, 0,Space.World);

        transform.position+=new Vector3(0,moveSpeed * Time.deltaTime,0);

        if (transform.position.y > 5.3f)
        {
            
            Destroy(gameObject);
            transform.parent.GetComponent<FloorManager>().CreateFloor();

        }
    }

    
}
