using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class CubeAgent : Agent
{
    public MeshRenderer planeMeshRenderer;
    public Material redMat;
    public Material greenMat;
    public Material groundMat;
    public GameObject enemyGo;
    public GameObject posionGo;
    public GameObject wallGo;
    private Animator anim;

    private Vector3 dir;

    public float moveSpeed;
    public bool isHeuristic;
    private void Start()
    {
        this.anim = this.GetComponent<Animator>();
    }
    public override void OnEpisodeBegin()
    {
        // 에피소드 시작

        //this.transform.localPosition = new Vector3(0, 0.5f, 0);
        //this.targetGo.transform.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);

        this.transform.localPosition = new Vector3(Random.Range(-2.5f, 2.5f), 0, Random.Range(-4.1f, 1f));
        enemyGo.transform.localPosition = new Vector3(Random.Range(-2.5f, 2.5f), 0.5f, Random.Range(-4.1f, 1f));
        posionGo.transform.localPosition = new Vector3(Random.Range(-2.5f, 2.5f), 0.5f, Random.Range(-4.1f, 1f));
        //wallGo.transform.localPosition = new Vector3(0, 0.5f, -1);

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 관찰
        sensor.AddObservation(this.transform.localPosition);    // 내위치 3
        sensor.AddObservation(this.enemyGo.transform.localPosition);   // 타겟 위치 3
        sensor.AddObservation(this.posionGo.transform.localPosition);   // 타겟 위치 3
        //sensor.AddObservation(this.wallGo.transform.localPosition);   // 벽 위치 3
        //sensor.AddObservation(this.rBody.velocity.x);   // 1
        //sensor.AddObservation(this.rBody.velocity.z);   // 1
        //sensor.AddObservation(this.transform.position.x);   // 1
        //sensor.AddObservation(this.transform.position.z);   // 1
        sensor.AddObservation(this.dir);   // 3
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // 매스텝마다 이동한다
        var discreateActions = actions.DiscreteActions;

        var h = discreateActions[0];
        var v = discreateActions[1];

        if (!isHeuristic)
        {
            h -= 1;
            v -= 1;
        }

        dir = new Vector3(h, 0, v);

        if (dir != Vector3.zero)
        {
            var angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
            this.transform.Translate(dir * this.moveSpeed * Time.deltaTime, Space.World);
        }



        //float dis = Vector3.Distance(this.targetGo.transform.localPosition, this.transform.localPosition);

    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // 사용자가 직접 조작

        var discreteAction = actionsOut.DiscreteActions;
        discreteAction[0] = (int)Input.GetAxisRaw("Horizontal");
        discreteAction[1] = (int)Input.GetAxisRaw("Vertical");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            AddReward(-0.5f);
            this.StartCoroutine(ChangeGroundColor(redMat));
            EndEpisode();
        }
        else if (collision.transform.CompareTag("Enemy"))
        {
            AddReward(-0.5f);
            this.StartCoroutine(ChangeGroundColor(redMat));
            EndEpisode();
        }
        else if (collision.transform.CompareTag("Posion"))
        {
            AddReward(0.1f);
            this.StartCoroutine(ChangeGroundColor(greenMat));
            EndEpisode();
        }
    }

    private IEnumerator ChangeGroundColor(Material mat)
    {
        this.planeMeshRenderer.material = mat;
        yield return new WaitForSeconds(0.5f);
        this.planeMeshRenderer.material = this.groundMat;
        yield return null;


    }
}
