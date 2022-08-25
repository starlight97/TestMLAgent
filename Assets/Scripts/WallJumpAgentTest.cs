using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;

public class WallJumpAgentTest : Agent
{
    public GameObject boxGo;
    public GameObject groundGo;
    public GameObject wallGo;
    private Renderer groundRenderer;
    public float moveSpeed;
    public float jumpForce;
    public Material redMat;
    public Material greenMat;
    public Material groundMat;

    private Rigidbody rBody;
    private Rigidbody boxRBody;
    private int m_Configuration;
    private bool isGrounded = true;

    private bool isBoxMove = false;
    private bool isJumpInBox = false;
    private int jumpCount = 0;
    EnvironmentParameters m_ResetParams;

    public float[,] wallHeightRange;

    private void Start()
    {
        this.rBody = this.GetComponent<Rigidbody>();
        this.boxRBody = boxGo.GetComponent<Rigidbody>();
        this.groundRenderer = this.groundGo.GetComponent<Renderer>();

        this.wallHeightRange = new float[3, 3];

        this.wallHeightRange[0, 0] = 0;
        this.wallHeightRange[0, 1] = 2;
        this.wallHeightRange[0, 2] = 3.5f;

        this.wallHeightRange[1, 0] = 2;
        this.wallHeightRange[1, 1] = 3;
        this.wallHeightRange[1, 2] = 4.0f;

        this.wallHeightRange[2, 0] = 2.5f;
        this.wallHeightRange[2, 1] = 3.5f;
        this.wallHeightRange[2, 2] = 4;

    }

    public override void Initialize()
    {
        this.m_ResetParams = Academy.Instance.EnvironmentParameters;
    }

    public override void OnEpisodeBegin()
    {
        //Box의 위치 x축으로 랜덤 초기화 
        //Agent 위치 초기화 
        //isGrounded 초기화 (default=true)
        var randX = UnityEngine.Random.Range(-2.5f, 2.5f);
        this.boxGo.transform.localPosition = new Vector3(randX, 0, -7.5f);
        this.transform.localPosition = new Vector3(0, 0, -11f);
        this.isGrounded = true;
        this.rBody.velocity = Vector3.zero;
        this.rBody.angularVelocity = Vector3.zero;
        this.boxRBody.velocity = Vector3.zero;
        this.boxRBody.angularVelocity = Vector3.zero;
        this.isBoxMove = false;
        this.isJumpInBox = false;
        this.jumpCount = 0;

        this.ConfigureAgent();

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        var dir = this.groundGo.transform.localPosition - this.transform.localPosition;
        
        sensor.AddObservation(dir.normalized);  //3
        sensor.AddObservation((this.isGrounded) ? 1 : 0);   //1
        sensor.AddObservation(this.boxGo.transform.position.z); //1

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(-0.0005f);

        //행동, 보상 
        var discreteActions = actions.DiscreteActions;

        var h = discreteActions[0] - 1;
        var v = discreteActions[1] - 1;

        Vector3 dir = new Vector3(h, 0, v);
        var angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        this.transform.Translate(dir * 2 * Time.deltaTime, Space.World);

        float disAgent_Box1 = Math.Abs(Vector3.Distance(this.transform.position, this.boxGo.transform.position));

        if (discreteActions[2] == 1 && this.isGrounded)
        {
            if (disAgent_Box1 >= 3f)
                AddReward(-0.1f);
            this.rBody.AddForce(Vector3.up * jumpForce);
            jumpCount++;
            if (jumpCount > 100)
            {
                EndEpisode();
            }
        }

        float disWall_Box = this.wallGo.transform.position.z - this.boxGo.transform.position.z;
        float disAgent_Box2 = Vector3.Distance(this.transform.position, this.boxGo.transform.position);


        if (Math.Abs(disWall_Box) <= 2.55f && isBoxMove == false)
        {
            AddReward(1f);
            isBoxMove = true;
        }
        if (Math.Abs(disAgent_Box2) < 1.5f && this.transform.position.y > 2 && isJumpInBox == false)
        {
            AddReward(1f);
            isJumpInBox = true;
        }

        //if ((!Physics.Raycast(m_AgentRb.position, Vector3.down, 20))
        //    || (!Physics.Raycast(m_ShortBlockRb.position, Vector3.down, 20)))

        if (this.transform.localPosition.y < -0.5f || this.boxGo.transform.localPosition.y < -0.5f)
        {
            //떨어졌을때 
            SetReward(-1f);
            StartCoroutine(this.ChangeGroundColor(this.redMat));
            EndEpisode();
        }
    }

    private IEnumerator ChangeGroundColor(Material mat)
    {
        this.groundRenderer.material = mat;
        yield return new WaitForSeconds(0.3f);
        this.groundRenderer.GetComponent<Renderer>().material = this.groundMat;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Goal") && this.isGrounded)
        {
            this.SetReward(1f);
            StartCoroutine(this.ChangeGroundColor(this.greenMat));
            EndEpisode();
        }
    }

    private void FixedUpdate()
    {
        this.CheckGround();
        //this.ConfigureAgent();
    }

    private void ConfigureAgent()
    {
        //var localScale = new Vector3(15, this.m_ResetParams.GetWithDefault("small_wall_height", 4), 1);
        //this.wallGo.transform.localScale = localScale;
        int heightIdx = Convert.ToInt32(this.m_ResetParams.GetWithDefault("small_wall_height", 0));
        var rand = UnityEngine.Random.Range(0,3);
        Vector3 newScale = this.wallGo.transform.localScale;
        newScale.y = wallHeightRange[heightIdx, rand];
        this.wallGo.transform.localScale = newScale;
        //this.wallGo.transform.localScale;

    }

    private void CheckGround()
    {
        RaycastHit hit;
        var ray = new Ray(transform.position + new Vector3(0, 0.05f, 0), -Vector3.up);
        //Debug.DrawRay(ray.origin, ray.direction * 3.1f, Color.red);

        Physics.Raycast(transform.position + new Vector3(0, 0.05f, 0), -Vector3.up, out hit, 0.1f);

        if (hit.collider != null
            && (hit.collider.CompareTag("walkableSurface") || hit.collider.CompareTag("Box") 
            && hit.normal.y > 0.95f))
        {
            this.isGrounded = true;
            this.rBody.velocity = Vector3.zero;
            this.rBody.angularVelocity = Vector3.zero;
        }
        else
        {
            this.isGrounded = false;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = (int)Input.GetAxisRaw("Horizontal") + 1;
        discreteActions[1] = (int)Input.GetAxisRaw("Vertical") + 1;

        if (Input.GetKey(KeyCode.Space))
        {
            discreteActions[2] = 1;
        }
        else
        {
            discreteActions[2] = 0;
        }

    }
}