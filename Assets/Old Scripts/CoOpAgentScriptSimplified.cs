using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class CoOpAgentScriptSimplified : Agent
{
    public float[,] searchArea;

    public float[,] agentSearchArea;
    private RaycastHit vision;
    public float rayLength = 2f;

    public Rigidbody agentRigidbody; 
    public int xDirection = 0;
    public int zDirection = 0;

    public void Start()
    {
        agentRigidbody = GetComponent<Rigidbody>();
        searchArea = new float[10,10];
    }

    public void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * rayLength, Color.red, 0.5f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {

        //sensor.AddObservation(agentRigidbody.velocity);
        Debug.DrawRay(transform.position, transform.forward * rayLength, Color.red, 0.5f);
        sensor.AddObservation(gameObject.transform.localPosition);
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
        if (zPosition > 9 || zPosition < 0 || xPosition > 9 || xPosition < 0 )
        {
            AddReward(-0.01f);
        }
        else if (searchArea[xPosition, zPosition] == 0f) {FoundSquare(xPosition, zPosition);}
        else
        {
            AddReward(-0.01f);
        }
        if(CheckSearchArea()){Finished();}
    }
    public override void AgentReset()
    {
        /*
        float resetXPosition = Random.value * 10f;
        float resetZPosition = Random.value * -10f;
        */
        gameObject.transform.localPosition = new Vector3(0.5f,0.2f,-0.5f);
        agentRigidbody.velocity = new Vector3(0f, 0f, 0f);
        searchArea = new float[10,10];
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        DestroyMiniMap();
    }
    public bool CheckSearchArea()
    {
        for( int i = 0; i <searchArea.GetLength(0);i++)
        {
            for (int j = 0; j < searchArea.GetLength(1); j++)
            {
                if(searchArea[i,j] == 0f){return false;}
            }
        }
        return true;
    }
    public void OnCollisionEnter(Collision collisionInfo)
    {
        //print ("collision with " + collisionInfo.collider.name);
        if(collisionInfo.collider.tag == "Agent")
        {
            agentSearchArea = collisionInfo.collider.gameObject.GetComponent<CoOpAgentScriptSimplified>().searchArea;
            for( int i = 0; i <agentSearchArea.GetLength(0);i++)
            {
                for (int j = 0; j < agentSearchArea.GetLength(0); j++)
                {
                    if(agentSearchArea[i,j] == 1f && searchArea[i,j] == 0f)
                    {
                        FoundSquare(i,j);
                    }
                }
            }
            //print("agent");
        }
        if(collisionInfo.collider.tag == "Wall")
        {
            agentRigidbody.velocity = new Vector3(0f, 0f, 0f);
            //print("wall");
        }
    }

    public override float[] Heuristic()
    {
        if (Input.GetKey(KeyCode.D))
        {
            return new float[] { 3 };
        }
        if (Input.GetKey(KeyCode.W))
        {
            return new float[] { 1 };
        }
        if (Input.GetKey(KeyCode.A))
        {
            return new float[] { 4 };
        }
        if (Input.GetKey(KeyCode.S))
        {
            return new float[] { 2 };
        }
        return new float[] { 0 };
    }

    public virtual void FoundSquare(int xPosition, int zPosition) 
    {
        searchArea[xPosition,zPosition] = 1f;
        AddReward(1f);
    }

    public virtual void DestroyMiniMap(){}

    public virtual void Finished()
    {
        GameObject parent = transform.parent.gameObject;
        Component[] CoopScript = parent.GetComponentsInChildren<CoOpAgentScriptSimplified>();
        foreach(CoOpAgentScriptSimplified script in CoopScript)
        {
            script.FinishedCalled(searchArea);
        }
    }

    public void FinishedCalled(float[,] finishedSearchArea)
    {
        for( int i = 0; i < finishedSearchArea.GetLength(0);i++)
        {
            for (int j = 0; j < finishedSearchArea.GetLength(0); j++)
            {
                if(finishedSearchArea[i,j] == 1f && searchArea[i,j] == 0f)
                {
                    searchArea[i,j] = 1f;
                    AddReward(1f);
                }
            }
        }
        Done();
    }
}
