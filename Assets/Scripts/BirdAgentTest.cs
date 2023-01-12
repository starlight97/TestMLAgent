using UnityEngine;
using UnityEngine.Events;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;

public class BirdAgentTest : Agent
{
    private Rigidbody2D rBody;
    public float jumpForce;
    public UnityAction onDie;
    public UnityAction<int> onSetLevel;
    
    EnvironmentParameters m_ResetParams;
    private int isJump;
    void Start()
    {
        this.rBody = this.GetComponent<Rigidbody2D>();
    }

    public void Init()
    {
        //this.jumpCoolTimeRoutine = null;
        int level = Convert.ToInt32(this.m_ResetParams.GetWithDefault("wall_height", 0));
        this.onSetLevel(level);

    }

    public override void OnEpisodeBegin()
    {
        Init();
    }
    public override void Initialize()
    {
        this.m_ResetParams = Academy.Instance.EnvironmentParameters;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 관찰
        sensor.AddObservation(this.transform.localPosition.y);    // 내위치 1
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //행동, 보상 
        var discreteActions = actions.DiscreteActions;

        int isJump = discreteActions[0];


        if (this.transform.localPosition.y <= -5.0f  || this.transform.localPosition.y >= 5.6f)
        {
            this.Die();
        }

        if (isJump == 1)
        {
            this.Jump();
            isJump = 0;
        }

        AddReward(0.1f);
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.Space))
        {
            discreteActions[0] = 1;
        }
        else
        {
            discreteActions[0] = 0;
        }
        this.onSetLevel(0);
    }

    private void Jump()
    {
        this.rBody.velocity = Vector3.up * this.jumpForce; 
    }
    private void Die()
    {
        AddReward(-1f);
        this.onDie();
        EndEpisode();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.Die();        
    }


}
