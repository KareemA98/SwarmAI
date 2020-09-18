using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube1x1 : MonoBehaviour
{
    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
       // Instantiate(cube1x1, new Vector3(5f,1f,-5f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn1x1(float x, float z)
    {
        GameObject newCube = Instantiate(cube, transform);
        newCube.transform.localPosition = new Vector3(x,1.5f,z);
    }
}
