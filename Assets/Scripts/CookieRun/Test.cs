using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CooKieRunTest
{
    public class Test : MonoBehaviour
    {
        public float jumpForce;
        private Rigidbody2D rBody;
        private float gravityScale;
        private bool isGround;

        private void Start()
        {
            this.rBody = this.GetComponent<Rigidbody2D>();
            this.gravityScale = this.rBody.gravityScale;
        }

        // Update is called once per frame
        void Update()
        {
            if(this.transform.localPosition.y <= 0)
            {
                Die();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.Jump();
            }
            HoleCheck();
        }

        private void HoleCheck()
        {
            //var ray = new Ray(this.transform.position + new Vector3(0, length, 0), Vector3.down);
            //Debug.DrawRay(ray.origin, ray.direction * 7f, Color.red, 0.5f);
            //RaycastHit2D raycastHit = Physics2D.Raycast(this.transform.position + new Vector3(0, length, 0), Vector3.down, 7f);  

            //if (raycastHit.collider !=null)
            //{
            //    Debug.Log(raycastHit.collider.name);
            //    Debug.Log(raycastHit.collider.tag);
            //}
        }

        private void Jump()
        {
            this.rBody.gravityScale = 1;
            this.rBody.velocity = Vector3.up * this.jumpForce;

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.collider.tag == "Floor" && this.rBody.gravityScale == 1)
            {
                var dis = Mathf.Abs(collision.gameObject.transform.localPosition.y - this.transform.localPosition.y);
                Debug.Log(dis);
                if (dis <= 2f)
                    this.rBody.gravityScale = this.gravityScale;
            }
        }

        private void Die()
        {
            this.StartCoroutine(DieRoutine());
        }

        private void Init()
        {
            this.rBody.velocity = Vector3.zero;

            this.transform.localPosition = new Vector3(6.6f, 2.35f, 0);
        }

        private IEnumerator DieRoutine()
        {
            yield return new WaitForSeconds(1f);
            Init();

        }

        //private void CheckGround()
        //{
        //    RaycastHit hit;
        //    var ray = new Ray(transform.position + new Vector3(0, 0.05f, 0), -Vector3.up);
        //    //Debug.DrawRay(ray.origin, ray.direction * 3.1f, Color.red);

        //    Physics.Raycast(transform.position + new Vector3(0, 0.05f, 0), -Vector3.up, out hit, 0.1f);

        //    if (hit.collider != null
        //        && (hit.collider.CompareTag("walkableSurface") || hit.collider.CompareTag("Box")
        //        && hit.normal.y > 0.95f))
        //    {
        //        this.isGrounded = true;
        //        this.rBody.velocity = Vector3.zero;
        //        this.rBody.angularVelocity = Vector3.zero;
        //    }
        //    else
        //    {
        //        this.isGrounded = false;
        //    }
        //}


    }
}

