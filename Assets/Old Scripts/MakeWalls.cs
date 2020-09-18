using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeWalls : MonoBehaviour
{
    public GameObject wall;
    public GameObject floor;
    public GameObject panel;
    public GameObject panel00;
    public GameObject cube1x1;
    public float xDistence;
    public float zDistence;
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.transform.parent.Find("Sphere Agent").GetComponent<CoOpAgentScript>().Finished();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public GameObject Spawn1x1(float x, float y, float z)
    {
        GameObject newCube = Instantiate(cube1x1, transform);
        newCube.transform.localPosition = new Vector3(x,y,z);
        return newCube;
    }

    public void printGrid(float[,] input){
        int rowLength = input.GetLength(0);
        int colLength = input.GetLength(1);

        string arrayString = "Grid Status:";
        arrayString += System.Environment.NewLine;
        for (int i = 0; i < rowLength; i++)
        {
            for (int j = 0; j < colLength; j++)
            {
                arrayString += string.Format("{0} ", input[i, j]);
            }
            arrayString += System.Environment.NewLine;
        }

        Debug.Log(arrayString);
    }
    
    public void MakeNewEnviroment(){
        StartCoroutine("MakeNewEnviroment1");
    }

    IEnumerator MakeNewEnviroment1()
    {
        counter++;
        if(counter % 1 == 1){yield break;}
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in panel00.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        
        xDistence = Random.Range(10,21);
        zDistence = Random.Range(10,21);

        float[,] grid = new float[(int)xDistence +2, (int)zDistence +2];
        for (int i = 0; i < grid.GetLength(0); i++){
            grid[i,0] = 2;
            grid[i,grid.GetLength(1)-1] = 2;
        }
        for (int i = 0; i < grid.GetLength(1); i++){
            grid[0,i] = 2;
            grid[grid.GetLength(0)-1,i] = 2;
        }

        printGrid(grid);

        GameObject newPanel;
        for (int x = 0; x < grid.GetLength(0); x++){
            for (int z = 0; z < grid.GetLength(1); z++){
                if(grid[x,z]==2){
                    Spawn1x1(x,0f,-z);
                };
                if(grid[x,z]==0){
                    newPanel = Instantiate(panel, panel00.transform);
                    newPanel.transform.localPosition = new Vector3(x,0,-z);
                };
                //yield return new WaitForSeconds(.005f);
            }
        }
    

        /*
        GameObject newWall =  Instantiate(wall, transform);
        newWall.transform.localScale = new Vector3(xDistence,2,0.5f);
        newWall.transform.localPosition = new Vector3(xDistence/2,0,0);
        GameObject newWall2 =  Instantiate(wall, transform);
        newWall2.transform.localScale = new Vector3(zDistence,2,0.5f);
        newWall2.transform.localPosition = new Vector3(0f,0,-zDistence/2);
        newWall2.transform.rotation = Quaternion.Euler(0,90,0);
        GameObject newWall3 =  Instantiate(wall, transform);
        newWall3.transform.localScale = new Vector3(xDistence,2,0.5f);
        newWall3.transform.localPosition = new Vector3(xDistence/2,0,-zDistence);
        GameObject newWall4 =  Instantiate(wall, transform);
        newWall4.transform.localScale = new Vector3(zDistence,2,0.5f);
        newWall4.transform.localPosition = new Vector3(xDistence,0,-zDistence/2);
        newWall4.transform.rotation = Quaternion.Euler(0,90,0);
        GameObject newFloor =  Instantiate(floor, transform);
        newFloor.transform.localScale = new Vector3(xDistence,1,zDistence);
        newFloor.transform.localPosition = new Vector3(xDistence/2,-1.5f,-zDistence/2);
        
        
        for(int i = 0; i < xDistence;i++)
        {
            for(int j = 0; j< zDistence; j++)
            {
                newPanel = Instantiate(panel, panel00.transform);
                newPanel.transform.localPosition = new Vector3(i +1f,0,-j-1f);
            }
        }
        */
    }
}
