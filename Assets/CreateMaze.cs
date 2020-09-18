using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class CreateMaze : MonoBehaviour
{
    int counter = 0;
    public GameObject people;
    public GameObject maze;
    public GameObject peoplePrefab;
    public GameObject mazePrefab;
    public GameObject emptyPersonPrefab;
    public GameObject randomMazePrefab;
    public GameObject[] mazePrefabArray;
    public GameObject[] peoplePrefabArray;
    public GameObject person;
    public bool test;
    public System.Diagnostics.Stopwatch startTime;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI[] percentages;
    public TextMeshProUGUI[] peoplesCanvas;
    public int numberOfAgents = 3;
    public Material floor;
    public Material wall;

    // Start is called before the first frame update


    // Update is called once per fram

    public void MakeMaze()
    {
        if(counter % numberOfAgents == 0)
        {
            Destroy(people);
            Destroy(maze);
            people = Instantiate(peoplePrefab, transform);
            maze = Instantiate(mazePrefab, transform);
            for(int i = 0; i < 400; i++){
                Transform quad = maze.transform.GetChild(i);

                foreach(Transform child in quad){
                    if(child.name.Contains("Door")){
                        child.gameObject.SetActive(false);
                    }
                    if(child.name.Contains("Quad")){
                        child.gameObject.GetComponent<MeshRenderer>().material = floor;
                    }
                    if(child.name.Contains("Wall")){
                        child.Find("Wall").gameObject.GetComponent<MeshRenderer>().material = wall;
                    }
                }
            }
            startTime = System.Diagnostics.Stopwatch.StartNew();
        }
        counter++;
    }
    public void Make10Maze()
    {
        if(counter % numberOfAgents == 0)
        {
            Destroy(people);
            Destroy(maze);
            int random = Random.Range(0, mazePrefabArray.Length);
            people = Instantiate(peoplePrefabArray[random], transform);
            maze = Instantiate(mazePrefabArray[random], transform);
            for(int i = 0; i < 400; i++){
                Transform quad = maze.transform.GetChild(i);

                foreach(Transform child in quad){
                    if(child.name.Contains("Door")){
                        child.gameObject.SetActive(false);
                    }
                    if(child.name.Contains("Quad")){
                        child.gameObject.GetComponent<MeshRenderer>().material = floor;
                    }
                    if(child.name.Contains("Wall")){
                        child.Find("Wall").gameObject.GetComponent<MeshRenderer>().material = wall;
                    }
                }
            }
            startTime = System.Diagnostics.Stopwatch.StartNew();
        }
        counter++;
    }
    public void MakeRandomMaze()
    {
        if(counter % numberOfAgents == 0)
        {
            Destroy(people);
            Destroy(maze);
            //people = Instantiate(peoplePrefabArray[random], transform);
            maze = Instantiate(randomMazePrefab, transform);
            people = Instantiate(emptyPersonPrefab, transform);
            maze.GetComponent<Maze>().Generate();

            for(int i = 0; i < 400; i++){
                Transform quad = maze.transform.GetChild(i);

                foreach(Transform child in quad){
                    if(child.name.Contains("Door")){
                        child.gameObject.SetActive(false);
                    }
                    if(child.name.Contains("Quad")){
                        child.gameObject.GetComponent<MeshRenderer>().material = floor;
                    }
                    if(child.name.Contains("Wall")){
                        child.Find("Wall").gameObject.GetComponent<MeshRenderer>().material = wall;
                    }
                }
            }
            
            for(int i = 0; i <5; i++)
		    {
			    GameObject newPerson = Instantiate(person, people.transform);
			    Transform position = maze.transform.GetChild(Random.Range(0,400));
			    newPerson.transform.localPosition = position.localPosition;
		    }
             startTime = System.Diagnostics.Stopwatch.StartNew();
        }
        counter++;
    }
    public void UpdateGraphics(string squaresFound, int agent, string peopleFound)
    {
        if(!test)
        {
            timer.text = (startTime.ElapsedMilliseconds / 1000).ToString();
            percentages[agent - 1].text = squaresFound;
            peoplesCanvas[agent - 1].text = peopleFound;
        }
    }
    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
