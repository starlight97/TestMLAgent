using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

namespace CooKieRunTest
{
    public class CookieRunAgent : Agent
    {
        public float jumpForce;
        private Rigidbody2D rBody;
        private float gravityScale;
        private bool isGround;
        private bool isJump = false;
        public UnityAction onDie;

        // Start is called before the first frame update
        void Start()
        {
            this.rBody = this.GetComponent<Rigidbody2D>();
            this.gravityScale = this.rBody.gravityScale;
        }

        public void Init()
        {
            this.rBody.velocity = Vector3.zero;

            this.transform.localPosition = new Vector3(6.6f, 2.35f, 0);
            isJump = false;
        }

        public override void OnEpisodeBegin()
        {
            Init();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            //sensor.AddObservation(this.transform.position.x); //1

        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            //행동, 보상 
            var discreteActions = actions.DiscreteActions;

            var actionCommand = discreteActions[0];

            if (actionCommand == 1 && !isJump)
            {
                isJump = true;
                this.Jump();
            }

            if (this.transform.localPosition.y <= 0)
            {
                Die();
            }
            AddReward(0.01f);

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

        }

        private void Jump()
        {
            this.rBody.gravityScale = 1;
            this.rBody.velocity = Vector3.up * this.jumpForce;

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.tag == "Floor" && this.rBody.gravityScale == 1)
            {
                var dis = Mathf.Abs(collision.gameObject.transform.localPosition.y - this.transform.localPosition.y);
                if (dis <= 2f)
                {
                    isJump = false;
                    this.rBody.gravityScale = this.gravityScale;
                }
            }
        }

        private void Die()
        {
            this.StartCoroutine(DieRoutine());
        }

        private IEnumerator DieRoutine()
        {
            yield return new WaitForSeconds(1f);
            AddReward(-1f);
            this.onDie();
            EndEpisode();
        }



    }

}
