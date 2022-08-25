using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class TestAgent : Agent
{
    private Rigidbody rBody;
    public GameObject targetGo;
    public GameObject wallGo;

    private void Start()
    {
        this.rBody = this.transform.GetComponent<Rigidbody>();
    }
    public override void OnEpisodeBegin()
    {
        // 에피소드 시작
        this.rBody.velocity = Vector3.zero; // 이동
        this.rBody.angularVelocity = Vector3.zero;  // 회전

        //this.transform.localPosition = new Vector3(0, 0.5f, 0);
        //this.targetGo.transform.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);

        this.transform.localPosition = new Vector3(0, 0.5f, -4);
        targetGo.transform.localPosition = new Vector3(0, 0.5f, 3f);
        wallGo.transform.localPosition = new Vector3(0, 0.5f, -1);

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 관찰
        sensor.AddObservation(this.transform.localPosition);    // 내위치 3
        sensor.AddObservation(this.targetGo.transform.localPosition);   // 타겟 위치 3
        sensor.AddObservation(this.wallGo.transform.localPosition);   // 벽 위치 3
        sensor.AddObservation(this.rBody.velocity.x);   // 1
        sensor.AddObservation(this.rBody.velocity.z);   // 1
    }

    public float moveForce = 10f;
    public override void OnActionReceived(ActionBuffers actions)
    {
        // 매스텝마다 이동한다
        Vector3 dir = Vector3.zero;
        dir.x = actions.ContinuousActions[0];
        dir.z = actions.ContinuousActions[1];

        this.rBody.AddForce(dir * moveForce);

        float dis = Vector3.Distance(this.targetGo.transform.localPosition, this.transform.localPosition);
        float wallDis = Vector3.Distance(this.wallGo.transform.localPosition, this.transform.localPosition);

        if(wallDis < 1.4f)
        {
            AddReward(-1.0f);
            EndEpisode();
        }

        if (dis < 3f)
        {
            AddReward(0.1f);
        }

        if (dis < 1.4f)
        {
            // 가까워졌을 경우 보상을 주고 에피소드를 종료 한다.
            AddReward(5.0f);
            EndEpisode();
        }
        else if(this.transform.localPosition.y < 0.1)
        {
            // 떨어졌다면 에피소드를 종료 한다
            AddReward(-3.0f);
            EndEpisode();
        }

    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // 사용자가 직접 조작
        var continuosAction = actionsOut.ContinuousActions;
        continuosAction[0] = Input.GetAxis("Horizontal");
        continuosAction[1] = Input.GetAxis("Vertical");
    }
}
