using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class HallwayAgentTest : Agent
{
    public MeshRenderer planeMeshRenderer;
    public Material redMat;
    public Material greenMat;
    public Material groundMat;

    public GameObject[] weaponGos;
    public GameObject[] weaponGoalGos;

    public float moveSpeed;
    public bool isHeuristic;

    private int answerNum;
    private int weaponCount;
    private Vector3 dir;

    private List<float> spawnRangeList;

    private void Start()
    {
        weaponCount = weaponGos.Length;
        spawnRangeList = new List<float>();
        spawnRangeList.Add(3f);
        spawnRangeList.Add(1f);
        spawnRangeList.Add(-1f);
        spawnRangeList.Add(-3f);
    }
    public void Init()
    {       
        for (int index = 0; index < weaponCount; index++)
        {
            weaponGos[index].SetActive(false);
        }

        this.transform.localPosition = new Vector3(0, 0, -5.5f);
        

        List<float> spawnRangeListClone = spawnRangeList.ToList();
        for (int index = 0; index < weaponCount; index++)
        {
            var randSpawnRangeIndex = Random.Range(0, spawnRangeListClone.Count);
            this.weaponGoalGos[index].transform.localPosition = new Vector3(spawnRangeListClone[randSpawnRangeIndex], 0.5f, 4);
            spawnRangeListClone.RemoveAt(randSpawnRangeIndex);
        }

        var rand = Random.Range(0, 4);
        answerNum = rand;
        weaponGos[answerNum].SetActive(true);

    }

    public override void OnEpisodeBegin()
    {
        // 에피소드 시작
        Init();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 관찰
        sensor.AddObservation(this.transform.localPosition);    // 내위치 3
        sensor.AddObservation(this.weaponGoalGos[answerNum].transform.localPosition);
        //sensor.AddObservation(this.weaponGoalGos[wrongrNum].transform.localPosition);
        //sensor.AddObservation(this.dir);   // 3
        sensor.AddObservation(StepCount / (float)MaxStep);

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
        AddReward(-1f / MaxStep);
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
        if (collision.transform.tag == "WeaponAxeGoal" || collision.transform.tag == "WeaponBowGoal" 
            || collision.transform.tag == "WeaponDaggerGoal" || collision.transform.tag == "WeaponShieldGoal")
        {

            if(collision.transform.name == weaponGoalGos[answerNum].name)
            {
                this.StartCoroutine(ChangeGroundColor(greenMat));
                SetReward(1f);
            }
            else
            {
                this.StartCoroutine(ChangeGroundColor(redMat));
                SetReward(-0.1f);
            }
            EndEpisode();

        }
        else if (collision.transform.tag == "Wall")
        {
            this.StartCoroutine(ChangeGroundColor(redMat));
            SetReward(-0.1f);
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
