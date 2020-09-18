using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject squareSearch;
    public GameObject sphereSearch;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowSquares()
    {
        squareSearch.SetActive(true);
        sphereSearch.SetActive(false);
    }
    public void ShowSpheres()
    {
        squareSearch.SetActive(false);
        sphereSearch.SetActive(true);   
    }
}
