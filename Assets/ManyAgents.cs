using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class ManyAgents : Agent
{
    public float[] searchArea;
    public float[] peopleArea;
    public float[] agentSearchArea;
    public float[] agentPeopleArea;
    public Rigidbody agentRigidbody; 
    public Transform mazeSquares;
    public Transform people;
    public CreateMaze mazeScript;
    public enum mazeTypeEnum {One, Ten, Random};
    public mazeTypeEnum mazeType;
    public int agent;
    private float numberOfAgents;

    //Observations are collected by sensors attached to the agent. It is run at regular intervals in order to make new decisions.
    public override void CollectObservations(VectorSensor sensor)
    {
        //The first vector observation is the amount of tiles found so far.
        //In our example the room always has 400 tiles, but the functions can adapt to larger / smaller rooms.
        int tilesFound = 0;
        for(int i = 0; i <searchArea.GetLength(0); i++)
        {
            if(searchArea[i] == 1f)
            {
                tilesFound++;
            }
        }
        sensor.AddObservation(tilesFound);

        //The second vector observation is the amount of 'people' found.
        int peopleFound = 0;
        for(int i = 0; i <peopleArea.GetLength(0); i++)
        {
            if(peopleArea[i] == 1f)
            {
                peopleFound++;
            }
        }

        //As well as the progress on finding tiles & people, it also takes in its own velocity, and an integer denoting which of the 3 agents it is.
        //It also sends the amount of tiles found to be displayed on the menu.
        sensor.AddObservation(agent);
        sensor.AddObservation(peopleFound);
        sensor.AddObservation(transform.InverseTransformDirection(agentRigidbody.velocity));
        mazeScript.UpdateGraphics((tilesFound/4).ToString(), agent, peopleFound.ToString());
    }

    //The agent's action is run every step, based on an integer returned from the observations being fed through the network.
    public override void AgentAction(float[] act)
    {
        //The range of actions it can take is simply whether to move up/down/left/right or to not move at all.
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

        //Each agent has the control to end the run, if it has reached a state where it has found every tile and person.
        if(CheckSearchArea()){Finished();}

        //Every step, a portion of the reward is removed. 
        //This means that while training, a run in which it finds all the tiles faster will get a higher reward.
        AddReward(-0.0001f);
    }

    // Once the run finishes or times out, the maze is reset (and randomised) and the agents are put in a starting position.
    public override void AgentReset()
    {
        //It can either just reset, generate as one of ten, or as an entirely randomised maze.
        //The agent's velocity and rotation is also halted to prevent any actions overrunning from the previous maze.
        searchArea = new float[400];
        peopleArea = new float[5];
        numberOfAgents = mazeScript.numberOfAgents;
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
        Vector3 pos = mazeSquares.GetChild(agent).transform.localPosition;
        pos.y = 0.5f;
        gameObject.transform.localPosition = pos;
        agentRigidbody.velocity = new Vector3(0f, 0f, 0f);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }

    //A check to see if the entire search area has been discovered, works for any size maze.
    //We can assume in our scenari we know the amount of tiles to be found beforehand.
    public bool CheckSearchArea()
    {
        for( int i = 0; i <searchArea.GetLength(0);i++)
        {
            if(searchArea[i] == 0f){return false;}
        }
        return true;
    }

    //When an agent enters the space of another agent, this is how information is exchanged.
    //They cross-reference each other's data on which tiles and people have been found, each agent gets the reward for this interraction as if it had found the tiles itself.
    public void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.collider.tag == "Agent")
        {
            agentSearchArea = collisionInfo.collider.gameObject.GetComponent<ManyAgents>().searchArea;
            for( int i = 0; i <agentSearchArea.GetLength(0);i++)
            {
                if(agentSearchArea[i] == 1f && searchArea[i] == 0f)
                {
                    FoundSquare(i);
                }
            }
            agentPeopleArea = collisionInfo.collider.gameObject.GetComponent<ManyAgents>().peopleArea;
            for( int i = 0; i <agentPeopleArea.GetLength(0);i++)
            {
                if(agentPeopleArea[i] == 1f && peopleArea[i] == 0f)
                {
                    FoundPerson(i);
                }
            }         
        }
    }
    public void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "NotFound")
        {
            if(searchArea[collider.transform.parent.GetSiblingIndex()] == 0f)
            {
                FoundSquare(collider.transform.parent.GetSiblingIndex());
            }
        }
    }

    //This enables manual control of the agents for testing purposes.

    //In order to visualise the progress of the run, this function changes the colour of each tile to reflect how many agents have found it.
    //It also sets the correct tile to be 'found' in the searchArea of each agent, and definies the reward given per tile.
    public virtual void FoundSquare(int position) 
    {
        searchArea[position] = 1f;
        Transform quad = mazeSquares.GetChild(position).GetChild(0);
        Color currentColor = quad.GetComponent<MeshRenderer>().material.color;
        float r = (currentColor.r + (-1f/numberOfAgents));
        float g = (currentColor.g + (1f/numberOfAgents));
        float b = currentColor.b;
        quad.GetComponent<MeshRenderer>().material.color = new Color(r,g,b);
        float reward = 0.05f;
        AddReward(reward);
    }

    //Similarly to FoundSquare, this function controls the visualisation of how many agents have found a person,and the reward given.
    public void FoundPerson(int position)
    {
        peopleArea[position] = 1f;
        Transform person = people.GetChild(position);
        Color currentColor = person.GetComponent<MeshRenderer>().material.color;
        float r = currentColor.r += (-1/numberOfAgents);
        float g = currentColor.g += (-1/numberOfAgents);
        float b = currentColor.b;
        person.GetComponent<MeshRenderer>().material.color = new Color(r,g,b);
        float reward = 1f;
        AddReward(reward);
    }

    //If any of the agents determine that the task is done, it must end the run for each other agent too, without them treating it like a timeout (where they didn't find each tile).
    public virtual void Finished()
    {
        GameObject parent = transform.parent.gameObject;
        Component[] CoopScript = parent.GetComponentsInChildren<ManyAgents>();
        foreach(ManyAgents script in CoopScript)
        {
            script.FinishedCalled(searchArea);
        }
    }

    //Controls the extra reward given for completing the task, and ends the run.
    public void FinishedCalled(float[] finishedSearchArea)
    {
        AddReward(2f);
        Done();
    }
    
    public override float[] Heuristic()
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