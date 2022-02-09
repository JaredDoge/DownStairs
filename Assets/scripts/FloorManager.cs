using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{

    [SerializeField] GameObject[] floorPrefabs;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void CreateFloor()
    {

        int random =Random.Range(0, floorPrefabs.Length);
        GameObject obj = Instantiate(floorPrefabs[random], transform);
        obj.transform.position = new Vector3(Random.Range(-3.6f, 3.6f), -4.3f, 0f);

    }
}
