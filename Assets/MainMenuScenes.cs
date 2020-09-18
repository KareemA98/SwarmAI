using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScenes : MonoBehaviour
{
    public GameObject[] cameraArray;
    public GameObject dropdown;
    public MazeAgent[] changeableSphereAgents;
    public MazeAgent[] resetAgents;
    public NewFinish newFinish;
    public void OneMazeTest()
    {
        cameraArray[0].SetActive(false);
        cameraArray[1].SetActive(true);
        cameraArray[2].SetActive(true);
        resetAgents[0].Finished();
    }
    public void TenMazeTest()
    {
        cameraArray[0].SetActive(false);
        cameraArray[3].SetActive(true);
        cameraArray[4].SetActive(true);
        resetAgents[1].Finished();
    }
    public void RandomMazeTest()
    {
        cameraArray[0].SetActive(false);
        cameraArray[5].SetActive(true);
        cameraArray[6].SetActive(true);
        resetAgents[2].Finished();
    }
    public void ThreeMaze()
    {
        cameraArray[0].SetActive(false);
        cameraArray[7].SetActive(true);
        cameraArray[8].SetActive(true);
        cameraArray[9].SetActive(true);
        cameraArray[10].SetActive(true);
        cameraArray[11].SetActive(true);
        cameraArray[12].SetActive(true);
        cameraArray[13].SetActive(true);
        resetAgents[3].Finished();
        resetAgents[4].Finished();
        resetAgents[5].Finished();
    }
    public void Maze1v2v3()
    {
        cameraArray[0].SetActive(false);
        cameraArray[14].SetActive(true);
        cameraArray[15].SetActive(true);
        cameraArray[16].SetActive(true);
        cameraArray[17].SetActive(true);
        cameraArray[18].SetActive(true);
        cameraArray[19].SetActive(true);
        cameraArray[20].SetActive(true);
        resetAgents[6].Finished();
        resetAgents[7].Finished();
        resetAgents[8].Finished();
    }
    public void Brains1v3()
    {
        cameraArray[0].SetActive(false);
        cameraArray[21].SetActive(true);
        cameraArray[22].SetActive(true);
        cameraArray[23].SetActive(true);
        cameraArray[24].SetActive(true);
        cameraArray[25].SetActive(true);
        resetAgents[9].Finished();
        resetAgents[10].Finished();
    }
    public void InfoRelay(){
        cameraArray[0].SetActive(false);
        cameraArray[26].SetActive(true);
        cameraArray[27].SetActive(true);
        newFinish.Finished();
    }
    public void BackToMainMenu()
    {
        foreach (GameObject camera in cameraArray)
        {
            camera.SetActive(false);
        }
        cameraArray[0].SetActive(true);
    }
    public void ChangeMap()
    {
        switch(dropdown.GetComponent<TMP_Dropdown>().value)
        {
            case 0:
                foreach(MazeAgent agent in changeableSphereAgents)
                {
                    agent.mazeType = MazeAgent.mazeTypeEnum.One;
                }
                break;
            case 1:
                foreach(MazeAgent agent in changeableSphereAgents)
                {
                    agent.mazeType = MazeAgent.mazeTypeEnum.Ten;
                }
                break;
            case 2:
                foreach(MazeAgent agent in changeableSphereAgents)
                {
                    agent.mazeType = MazeAgent.mazeTypeEnum.Random;
                }
                break;
        }
        changeableSphereAgents[0].Finished();
        changeableSphereAgents[3].Finished();
        changeableSphereAgents[6].Finished();
    }
}
