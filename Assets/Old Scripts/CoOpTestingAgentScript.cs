using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CoOpTestingAgentScript : CoOpAgentScript
{

    public GameObject miniMapSquare;
    public GameObject miniMapZero;

    public override void FoundSquare(int xPosition, int zPosition)
    {
        GameObject square = Instantiate(miniMapSquare,miniMapZero.transform);
        square.transform.localPosition = new Vector3(20*zPosition, 20*xPosition);
        searchArea[xPosition,zPosition] = 1f;
        AddReward(1f);
    }
    public override void DestroyMiniMap()
    {
        foreach (Transform child in miniMapZero.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    public override void Finished()
    {
        GameObject parent = transform.parent.gameObject;
        CoOpTestingAgentScript[] CoopScript = parent.GetComponentsInChildren<CoOpTestingAgentScript>();
        foreach(CoOpTestingAgentScript script in CoopScript)
        {
            script.FinishedCalled(searchArea);
        }
    }
}
