using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Maze mazePrefab;

	public Player playerPrefab;
	public Transform people;
	public GameObject person;

	public Maze mazeInstance;

	private Player playerInstance;
	private int counter;
	private bool first = true;

	
	private void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			RestartGame();
		}
	}

	public void BeginGame () {
		counter++;
        if(counter % 3 == 0 || counter % 3 == 2){return;}
		if(first) {first = false;}
		else{DestroyMaze();}
		Camera.main.clearFlags = CameraClearFlags.Skybox;
		//Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
		//mazeInstance = Instantiate(mazePrefab, gameObject.transform.parent) as Maze;
		mazeInstance.Generate();
		//playerInstance = Instantiate(playerPrefab) as Player;
		//playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
		//Camera.main.clearFlags = CameraClearFlags.Skybox;
		//Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);
		foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("Door"))
		{
			fooObj.SetActive(false);
		}
		for(int i = 0; i <5; i++)
		{
			GameObject newPerson = Instantiate(person, people);
			Transform position = mazeInstance.transform.GetChild(Random.Range(0,400));
			newPerson.transform.localPosition = position.localPosition;
		}
		
	}

	public void DestroyMaze()
	{
		foreach (Transform child in mazeInstance.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
		foreach (Transform child in people.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

	}

	private void RestartGame () {
		StopAllCoroutines();
		Destroy(mazeInstance.gameObject);
		if (playerInstance != null) {
			Destroy(playerInstance.gameObject);
		}
		BeginGame();
	}
}