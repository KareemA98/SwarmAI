﻿using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class NewFinish : Agent
{
    public float[] searchArea;
    public float[] peopleArea;
    public LayerMask rayLayerMask = Physics.DefaultRaycastLayers;
    public Vector3[] angles = {new Vector3(0f,0f,1f), new Vector3(0f,0f,-1f), new Vector3(1f,0f,0f), new Vector3(-1f,0f,0f), new Vector3(1f,0f,1f), new Vector3(-1f,0f,1f),new Vector3(1f,0f,-1f), new Vector3(-1f,0f,-1f)};
    public float[] agentSearchArea;
    public float[] agentPeopleArea;

    public Rigidbody agentRigidbody; 
    public Transform mazeSquares;
    public Transform people;
    public CreateMaze mazeScript;
    public int agent;
    private bool first = true;
    public enum mazeTypeEnum {One, Ten, Random};
    public mazeTypeEnum mazeType;
    GameObject square;

    void Start()
    {
        
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        if(agent == 4){
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }else
        {
            try
            {
                if(mazeSquares != null)
                {
                   int xcoord = Mathf.FloorToInt(transform.localPosition.x)+10;
                    int zcoord = Mathf.FloorToInt(transform.localPosition.z)+10;
                    string s = "Maze Cell ";
                    s += xcoord.ToString();
                    s += ", ";
                    s += zcoord.ToString();
                    square = mazeSquares.transform.Find(s).gameObject; 

                    for(int i = 1; i<5; i++){
                        if(square.transform.GetChild(i).eulerAngles == new Vector3(0f,0f,0f)){
                            if(square.transform.GetChild(i).name.Contains("Maze Wall")){
                                sensor.AddObservation(1f);
                            } else {
                                sensor.AddObservation(0f);
                            }
                        }
                        if(square.transform.GetChild(i).eulerAngles == new Vector3(0f,90f,0f)){
                            if(square.transform.GetChild(i).name.Contains("Maze Wall")){
                                sensor.AddObservation(1f);
                            } else {
                                sensor.AddObservation(0f);
                            }
                        }
                        if(square.transform.GetChild(i).eulerAngles == new Vector3(0f,180f,0f)){
                            if(square.transform.GetChild(i).name.Contains("Maze Wall")){
                                sensor.AddObservation(1f);
                            } else {
                                sensor.AddObservation(0f);
                            }
                        }
                        if(square.transform.GetChild(i).eulerAngles == new Vector3(0f,270f,0f)){
                            if(square.transform.GetChild(i).name.Contains("Maze Wall")){
                                sensor.AddObservation(1f);
                            } else {
                                sensor.AddObservation(0f);
                            }
                        }
                    }    
                }     
            }
            catch (MissingReferenceException)
            {
                print("MissRefExc");
                sensor.AddObservation(0f);
                sensor.AddObservation(0f);
                sensor.AddObservation(0f);
                sensor.AddObservation(0f);
                mazeSquares = mazeScript.maze.transform;
                people = mazeScript.people.transform;
            }

            int counter1 = 0;
            for(int i = 0; i <searchArea.GetLength(0); i++)
            {
                if(searchArea[i] == 1f)
                {
                    counter1++;
                }
            }
            sensor.AddObservation(counter1);
            int counter = 0;
            for(int i = 0; i <peopleArea.GetLength(0); i++)
            {
                if(peopleArea[i] == 1f)
                {
                    counter++;
                }
            }
            sensor.AddObservation(agent);
            sensor.AddObservation(counter);
            sensor.AddObservation(transform.InverseTransformDirection(agentRigidbody.velocity));
            mazeScript.UpdateGraphics((counter1/4).ToString(), agent, counter.ToString());
        }
    }

    public override void AgentAction(float[] act)
    {
        if(agent == 4){
            if(CheckSearchArea()){Finished();}
        }else
        {
            var dirToGo = Vector3.zero;
            var rotateDir = Vector3.zero;
            var forwardAxis = Mathf.FloorToInt(act[0]);
            switch (forwardAxis)
            {
                case 0:
                    dirToGo = Vector3.zero;
                    break;
                case 1:
                    dirToGo = Vector3.forward;
                    break;
                case 2:
                    dirToGo = -Vector3.forward;
                    break;
                case 3:
                    dirToGo = Vector3.right;
                    break;
                case 4:
                    dirToGo = -Vector3.right;
                    break;
            }
            agentRigidbody.AddForce(dirToGo * 0.5f, ForceMode.VelocityChange);
            for(int i = 0; i < angles.GetLength(0); i++)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position,transform.TransformDirection(angles[i]) , out hit, 5f , rayLayerMask))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(angles[i]) * hit.distance, Color.yellow);
                //Debug.Log("Did Hit");
                    if(hit.collider.tag == "NotFound")
                    {   
                //print("trigger");
                        if(searchArea[hit.collider.transform.parent.GetSiblingIndex()] == 0f)
                        {
                            FoundSquare(hit.collider.transform.parent.GetSiblingIndex(), false);
                        }
                    }
                    if(hit.collider.tag == "Person")
                    {
                        if(peopleArea[hit.collider.transform.GetSiblingIndex()] == 0f)
                        {
                            FoundPerson(hit.collider.transform.GetSiblingIndex(), false);
                        }
                    }
                }
            }
            AddReward(-0.00001f);
        }
        
    }
    public override void AgentReset()
    {
        searchArea = new float[400];
        peopleArea = new float[5];
        switch(mazeType)
        {
            case mazeTypeEnum.One:
            mazeScript.MakeMaze();
            break;
            case mazeTypeEnum.Ten:
            mazeScript.Make10Maze();
            break;
            case mazeTypeEnum.Random:
            mazeScript.MakeRandomMaze();
            break;
        }
        mazeSquares = mazeScript.maze.transform;
        people = mazeScript.people.transform;
        //print("reset");
        gameObject.transform.localPosition = new Vector3(-9.5f, 0.5f, -9.5f);
        agentRigidbody.velocity = new Vector3(0f, 0f, 0f);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }
    public bool CheckSearchArea()
    {
        for( int i = 0; i <searchArea.GetLength(0);i++)
        {
            if(searchArea[i] == 0f){return false;}
        }
        return true;
    }

    public void OnCollisionEnter(Collision collisionInfo)
    {
        //print ("collision with " + collisionInfo.collider.name);
        if(collisionInfo.collider.tag == "Agent")
        {
            agentSearchArea = collisionInfo.collider.gameObject.GetComponent<NewFinish>().searchArea;
            for( int i = 0; i <agentSearchArea.GetLength(0);i++)
            {
                if(agentSearchArea[i] == 1f && searchArea[i] == 0f)
                {
                    if(agent == 4 || collisionInfo.collider.gameObject.GetComponent<NewFinish>().agent == 4){
                        FoundSquare(i, true);
                    } else {
                        FoundSquare(i, false);
                    } 
                }
            }
            agentPeopleArea = collisionInfo.collider.gameObject.GetComponent<NewFinish>().peopleArea;
            for( int i = 0; i <agentPeopleArea.GetLength(0);i++)
            {
                if(agentPeopleArea[i] == 1f && peopleArea[i] == 0f)
                {
                    if(agent == 4 || collisionInfo.collider.gameObject.GetComponent<NewFinish>().agent == 4){
                        FoundPerson(i, true);
                    } else {
                        FoundPerson(i, false);
                    } 
                }
            }         
        }
    }

    public override float[] Heuristic()
    {
        if(agent == 4){
            return new float[] { 0 };
        }else
        {
            if (Input.GetKey(KeyCode.W))
            {
                return new float[] { 3 };
            }
            if (Input.GetKey(KeyCode.A))
            {
                return new float[] { 1 };
            }
            if (Input.GetKey(KeyCode.S))
            {
                return new float[] { 4 };
            }
            if (Input.GetKey(KeyCode.D))
            {
                return new float[] { 2 };
            }
            return new float[] { 0 };
        }
    }

    public virtual void FoundSquare(int position, bool a) 
    {
        try{
            if(mazeSquares != null)
            {
                searchArea[position] = 1f;
                Transform quad = mazeSquares.GetChild(position).GetChild(0);
                MazeCell cell = quad.parent.GetComponent<MazeCell>();
                
                if(quad.GetComponent<MeshRenderer>().material.color == Color.red)
                {
                    quad.GetComponent<MeshRenderer>().material.color = Color.yellow;
                }
                else if(quad.GetComponent<MeshRenderer>().material.color == Color.yellow)
                {
                    quad.GetComponent<MeshRenderer>().material.color = Color.green;
                }
                else if(quad.GetComponent<MeshRenderer>().material.color == Color.green)
                {
                    quad.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
                else
                {
                    quad.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                int layer = quad.gameObject.layer;
                if (layer == 9)
                {
                    quad.gameObject.layer = 9 + agent;
                }
                else if(layer == 10 || layer == 11 || layer == 12)
                {
                    quad.gameObject.layer = (layer + 9 + agent);
                }
                else
                {
                    quad.gameObject.layer = 0;
                }
            }
        }
        catch (MissingReferenceException)
        {
            print("MissRefExc");
            mazeSquares = mazeScript.maze.transform;
            people = mazeScript.people.transform;
        }
        
        if(a){
            float reward = 0.5f;
            AddReward(reward);
        } else {
            float reward = 0.05f;
            AddReward(reward);
        } 
    }

    public void FoundPerson(int position, bool a)
    {
        try{
            if(mazeSquares != null)
            {
                peopleArea[position] = 1f;
                Transform person = people.GetChild(position);
                
                if(person.GetComponent<MeshRenderer>().material.color == Color.red)
                {
                    person.GetComponent<MeshRenderer>().material.color = Color.yellow;
                }
                else if(person.GetComponent<MeshRenderer>().material.color == Color.yellow)
                {
                    person.GetComponent<MeshRenderer>().material.color = Color.green;
                }
                else if(person.GetComponent<MeshRenderer>().material.color == Color.green)
                {
                    person.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
                else
                {
                    person.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                int layer = person.gameObject.layer;
                if (layer == 9)
                {
                    person.gameObject.layer = 9 + agent;
                }
                else if(layer == 10 || layer == 11 || layer == 12)
                {
                    person.gameObject.layer = (layer + 9 + agent);
                }
                else
                {
                    person.gameObject.layer = 0;
                }
            }
        }
        catch (MissingReferenceException)
        {
            print("MissRefExc");
            mazeSquares = mazeScript.maze.transform;
            people = mazeScript.people.transform;
        }

        if(a){
            float reward = 10f;
            AddReward(reward);
        } else {
            float reward = 1f;
            AddReward(reward);
        } 
    }

    public virtual void DestroyMiniMap(){}

    public virtual void Finished()
    {
        GameObject parent = transform.parent.gameObject;
        Component[] CoopScript = parent.GetComponentsInChildren<NewFinish>();
        foreach(NewFinish script in CoopScript)
        {
            script.FinishedCalled(searchArea);
        }
    }

    public void FinishedCalled(float[] finishedSearchArea)
    {
        /*
        for( int i = 0; i < finishedSearchArea.GetLength(0);i++)
        {
            if(finishedSearchArea[i] == 1f && searchArea[i] == 0f)
            {
                FoundSquare(i);
                //searchArea[i,j] = 1f;
                //AddReward(1f);
            }
        }
        */
    AddReward(2f);
    Done();
    }

}
