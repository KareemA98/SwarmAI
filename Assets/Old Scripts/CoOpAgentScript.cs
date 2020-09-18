using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class CoOpAgentScript : Agent
{
    public float[,] searchArea;

    public float[,] agentSearchArea;


    public Rigidbody agentRigidbody; 
    public GameObject panel00;
    public GameObject Room;
    public int xDirection = 0;
    public int zDirection = 0;
    public int xSize = 10;
    public int zSize = 10;
    private float blockCounter = 1;

    public void Start()
    {
        agentRigidbody = GetComponent<Rigidbody>();
        /*
        int[] xAndZ = Room.GetComponent<MakeWalls>().MakeNewEnviroment();
        xSize =xAndZ[0]
        zSize = xAndZ[1];
        */
        searchArea = new float[10,10];
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        /*
        float front, back, right, left;
        front = back = right = left = 0;
        RaycastHit hit;
        Physics.Raycast(transform.position, new Vector3(1, -0.8f, 0), out hit,3f);
        if (hit.collider.CompareTag("NotFound")){front = 1;}
        else if(hit.collider.CompareTag("Wall")){front = 0.5f;}
        Physics.Raycast(transform.position, new Vector3(-1, -0.8f, 0), out hit,3f);
        if (hit.collider.CompareTag("NotFound")){back = 1;}
        else if(hit.collider.CompareTag("Wall")){back = 0.5f;}
        Physics.Raycast(transform.position, new Vector3(0, -0.8f, 1), out hit,3f);
        if (hit.collider.CompareTag("NotFound")){right = 1;}
        else if(hit.collider.CompareTag("Wall")){back = 0.5f;}
        Physics.Raycast(transform.position, new Vector3(0, -0.8f, -1), out hit,3f);
        if (hit.collider.CompareTag("NotFound")){left = 1;}
        else if(hit.collider.CompareTag("Wall")){back = 0.5f;}
        Debug.DrawRay(transform.position,  new Vector3(1, -0.7f, 0) * 3f, Color.yellow);
        Debug.DrawRay(transform.position,  new Vector3(-1, -0.7f, 0) * 3f, Color.red);
        Debug.DrawRay(transform.position,  new Vector3(0, -0.7f, 1) * 3f, Color.green);
        Debug.DrawRay(transform.position,  new Vector3(0, -0.7f, -1) * 3f, Color.blue);
        */
        for( int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                if (i < xSize && j < zSize )
                {
                    sensor.AddObservation( searchArea[i,j] );
                }
                else
                {
                    sensor.AddObservation(0f);
                }
            }
        }
        //sensor.AddObservation(agentRigidbody.velocity);
        /*
        sensor.AddObservation(front);
        sensor.AddObservation(back);
        sensor.AddObservation(right);
        sensor.AddObservation(left);
        */
        sensor.AddObservation(gameObject.transform.parent.GetChild(0).transform.localPosition);
        //sensor.AddObservation(gameObject.transform.parent.GetChild(1).transform.localPosition);
        sensor.AddObservation(transform.InverseTransformDirection(agentRigidbody.velocity));
    }
    public override void AgentAction(float[] act)
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
        /*Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = -vectorAction[1];
        transform.Translate(controlSignal * 0.1f);
        agentRigidbody.AddForce(controlSignal * 5f);
        */
        int xPosition = Mathf.FloorToInt(gameObject.transform.localPosition.x);
        int zPosition = Mathf.FloorToInt(-(gameObject.transform.localPosition.z));
        /*if (zPosition > 9 || zPosition < 0 || xPosition > 9 || xPosition < 0 )
        {
            FinishedEarly();
        }
        */
        /*if(gameObject.transform.localPosition.y > 0.5 || gameObject.transform.localPosition.y < 0)
        {
            Done();
        }
        */
        //print(searchArea.Length);
        /*
        if (zPosition > zSize -1 || zPosition < 0 || xPosition > xSize - 1 || xPosition < 0 )
        {
            AddReward(-0.1f);
        }
        else if (searchArea[xPosition, zPosition] == 0f) {FoundSquare(xPosition, zPosition);}
        else
        {
            AddReward(-0.1f);
        }
        */
        if (zPosition >= zSize  || zPosition < 0 || xPosition >= xSize  || xPosition < 0 )
        {
            //AddReward(-2f/maxStep);
        }
        else if (searchArea[xPosition, zPosition] == 0.5f) {FoundSquare(xPosition, zPosition);}
        if(CheckSearchArea()){Finished();}
        //AddReward(-1f/ maxStep);
    }
    public override void AgentReset()
    {
        Room.GetComponent<MakeWalls>().MakeNewEnviroment();
        gameObject.transform.localPosition = new Vector3(0.5f,0.2f,-0.5f);
        agentRigidbody.velocity = new Vector3(0f, 0f, 0f);
        xSize = (int)Room.GetComponent<MakeWalls>().xDistence;
        zSize = (int)Room.GetComponent<MakeWalls>().zDistence;
        float resetXPosition = Random.value * xSize;
        float resetZPosition = Random.value * -zSize;
        gameObject.transform.localPosition = new Vector3(1f,0.2f,-1f);
        searchArea = new float[xSize,zSize];
        blockCounter = 1;
        for( int i = 0; i <searchArea.GetLength(0);i++)
        {
            for (int j = 0; j < searchArea.GetLength(1); j++)
            {
                searchArea[i,j] = 0.5f;
            }
        }
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        DestroyMiniMap();
    }

    public bool CheckSearchArea()
    {
        for( int i = 0; i <searchArea.GetLength(0);i++)
        {
            for (int j = 0; j < searchArea.GetLength(1); j++)
            {
                if(searchArea[i,j] == 0.5f){return false;}
            }
        }
        return true;
    }

    public void OnCollisionEnter(Collision collisionInfo)
    {
        //print ("collision with " + collisionInfo.collider.name);
        if(collisionInfo.collider.tag == "Agent")
        {
            agentSearchArea = collisionInfo.collider.gameObject.GetComponent<CoOpAgentScript>().searchArea;;
            for( int i = 0; i <agentSearchArea.GetLength(0);i++)
            {
                for (int j = 0; j < agentSearchArea.GetLength(1); j++)
                {
                    if(agentSearchArea[i,j] == 1f && searchArea[i,j] == 0.5f)
                    {
                        FoundSquare(i,j);
                    }
                }
            }
            //print("agent");
        }
        /*
        if(collisionInfo.collider.tag == "Wall")
        {
            agentRigidbody.velocity = new Vector3(0f, 0f, 0f);
            //print("wall");
        }
        */
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

    public virtual void FoundSquare(int xPosition, int zPosition) 
    {
        searchArea[xPosition,zPosition] = 1f;
        //AddReward(1f);
        int child = xPosition*zSize +zPosition;
        //print(child);
        panel00.transform.GetChild(child).GetComponent<MeshRenderer>().material.color = Color.red;
        panel00.transform.GetChild(child).tag = "Found";
        float totalSquares = xSize*zSize;
        //float reward = (2*blockCounter)/((totalSquares*(totalSquares+1))/2);
        float reward = 2/totalSquares;
        AddReward(reward);
        //blockCounter++;
    }

    public virtual void DestroyMiniMap(){}

    public virtual void Finished()
    {
        GameObject parent = transform.parent.gameObject;
        Component[] CoopScript = parent.GetComponentsInChildren<CoOpAgentScript>();
        foreach(CoOpAgentScript script in CoopScript)
        {
            script.FinishedCalled(searchArea);
        }
    }

    public void FinishedCalled(float[,] finishedSearchArea)
    {
        for( int i = 0; i < finishedSearchArea.GetLength(0);i++)
        {
            for (int j = 0; j < finishedSearchArea.GetLength(1); j++)
            {
                if(finishedSearchArea[i,j] == 1f && searchArea[i,j] == 0.5f)
                {
                    FoundSquare(i,j);
                    //searchArea[i,j] = 1f;
                    //AddReward(1f);
                }
            }
        }
        //SetReward(2f);
        Done();
    }
}
