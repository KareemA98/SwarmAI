using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class AgentScript : Agent
{
    public float[,] searchArea;
    //public GameObject miniMapSquare;
    //public GameObject miniMapZero;
    Rigidbody agentRigidbody; 
    public int xDirection = 0;
    public int zDirection = 0;
    void Start()
    {
        agentRigidbody = GetComponent<Rigidbody>();
        searchArea = new float[10,10];
    }
    public override void InitializeAgent()
    {
        //m_ResetParams = Academy.Instance.FloatProperties;
        //SetResetParameters();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        for( int i = 0; i < searchArea.GetLength(0); i++)
        {
            for (int j = 0; j < searchArea.GetLength(1); j++)
            {
                sensor.AddObservation( searchArea[i,j] );
            }
        }
        //sensor.AddObservation(agentRigidbody.velocity);
        sensor.AddObservation(gameObject.transform.localPosition);
        sensor.AddObservation(transform.InverseTransformDirection(agentRigidbody.velocity));
    }
    public override void AgentAction(float[] vectorAction)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = Mathf.FloorToInt(vectorAction[0]);
        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
        }
        transform.Rotate(rotateDir, Time.deltaTime * 200f);
        agentRigidbody.AddForce(dirToGo * 2f, ForceMode.VelocityChange);
        /*Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = -vectorAction[1];
        transform.Translate(controlSignal * 0.1f);
        agentRigidbody.AddForce(controlSignal * 5f);
        */
        int xPosition = Mathf.FloorToInt(gameObject.transform.localPosition.x);
        int zPosition = Mathf.FloorToInt(-(gameObject.transform.localPosition.z));
        if (zPosition > 10 || zPosition < 0 || xPosition > 10 || xPosition < 0 )
        {
            print(xPosition);
            print(zPosition);
            SetReward(-1f);
            Done();
        }
        /*if(gameObject.transform.localPosition.y > 0.5 || gameObject.transform.localPosition.y < 0)
        {
            Done();
        }
        */
        if (searchArea[xPosition, zPosition] == 0f)
        {
            //GameObject square = Instantiate(miniMapSquare,miniMapZero.transform);
            //square.transform.localPosition = new Vector3(20*zPosition, 20*xPosition);
            searchArea[xPosition,zPosition] = 1f;
            AddReward(1f);
        }
        else
        {
            AddReward(-0.01f);
        }
        if(CheckSearchArea()){Done();}
    }
    public override void AgentReset()
    {
        gameObject.transform.localPosition = new Vector3(0.5f,0.2f,-0.5f);
        agentRigidbody.velocity = new Vector3(0f, 0f, 0f);
        searchArea = new float[10,10];
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
        //foreach (Transform child in miniMapZero.transform)
        //{
        //    GameObject.Destroy(child.gameObject);
        //}
        //Reset the parameters when the Agent is reset.
    }
    bool CheckSearchArea()
    {
        for( int i = 0; i <searchArea.GetLength(0);i++)
        {
            for (int j = 0; j < searchArea.GetLength(0); j++)
            {
                if(searchArea[i,j] == 0f){return false;}
            }
        }
        return true;
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
}
